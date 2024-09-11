using Application.Abstractions.Services.SignalRServices;
using Application.DTOs.SignalRDTOs;
using Application.UnitOfWorks;
using Domain.BaseModels;
using Domain.Entities.ConcretEntities;
using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using SignalR.Helpers;
using SignalR.Hubs;
using System.Reflection;

namespace SignalR.Implementions.Services;

public class HubConnectionsHandler : IHubConnectionsHandler
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHubContext<ChatHub> hub;

    public HubConnectionsHandler(IHttpContextAccessor contextAccessor, IUnitOfWork unitOfWork, IHubContext<ChatHub> hub)
    {
        _contextAccessor = contextAccessor;
        _unitOfWork = unitOfWork;
        this.hub = hub;
    }

    public async Task OnConnected(string connectionId)
    {
        var user = await _unitOfWork.GetReadRepository<AppUser, int>().GetByIdAsync(GetUserIdByToken());
        user.ConnectionId = connectionId;
        user.IsOnline = true;
        await _unitOfWork.Commit();
        await SendAll("UserConnected", user.Id.ToString());

    }

    public async Task OnDisconnected()
    {

        var user = await _unitOfWork.GetReadRepository<AppUser, int>().GetByIdAsync(GetUserIdByToken());
        user.ConnectionId = null;
        user.IsOnline = false;
        user.LastActivityDate = DateTime.UtcNow;
        await _unitOfWork.Commit();
        await SendAll("UserDisconnected", user.Id.ToString());
    }

    private async Task SendAll(string method, string param) =>
            await hub.Clients.All.SendAsync(method, param);

    private int GetUserIdByToken()
    {
        var token = _contextAccessor.HttpContext.Request.Query["access_token"];
        var userId = JwtDecryptor.Decrypt(token!);
        return int.Parse(userId!);
    }

    public async Task SendMessage(MessageDTO model)
    {
        var userMessages = _unitOfWork.GetWriteRepository<UsersMessages, int>();
        await userMessages.AddAsync(new()
        {
            CreatedDate = DateTime.UtcNow,
            FromUserId = GetUserIdByToken(),
            ToUserId = (int)model.toUserId!,
            Message = new Message()
            {
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                Content = model.Message,
                MessageType = model.MessageType
            }
        });

        var toUser = await _unitOfWork.GetReadRepository<AppUser, int>().GetByIdAsync((int)model.toUserId);
        model.toUserId = GetUserIdByToken();
        model.IsSender = !model.IsSender;
        await hub.Clients.Client(toUser.ConnectionId).SendAsync("ReceivedMessage", model);
        await _unitOfWork.Commit();
    }


}

