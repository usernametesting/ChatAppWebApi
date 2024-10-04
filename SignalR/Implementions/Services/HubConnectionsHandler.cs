using Application.Abstractions.Services.ControllerServices;
using Application.Abstractions.Services.SignalRServices;
using Application.DTOs.Common;
using Application.DTOs.SignalRDTOs;
using Application.Helpers.Users;
using Application.UnitOfWorks;
using Domain.BaseModels;
using Domain.Entities.ConcretEntities;
using Domain.Enums.MessageEnums;
using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using SignalR.Helpers;
using SignalR.Hubs;
using System.Net;
using System.Reflection;

namespace SignalR.Implementions.Services;

public class HubConnectionsHandler : IHubConnectionsHandler
{
    private readonly IHubContext<ChatHub> hub;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserHelper _helper;

    public HubConnectionsHandler(IHubContext<ChatHub> hub, IUnitOfWork unitOfWork, IUserHelper helper)
    {
        this.hub = hub;
        _unitOfWork = unitOfWork;
        _helper = helper;
    }

    public async Task OnConnected(string connectionId)
    {
        var user = await _unitOfWork.GetReadRepository<AppUser, int>().GetByIdAsync(_helper.GetUserIdByToken());
        user.ConnectionId = connectionId;
        user.IsOnline = true;
        var updateValues = new Dictionary<string, object>
            {
                { "State", MessageState.NOTIFIED},
            };
        await _unitOfWork.GetWriteRepository<Message, int>()
                       .UpdateMultipleAsync(updateValues, m => m.UserMessages.ToUserId == user.Id
                                            && m.UserMessages.Message.State != MessageState.SEEN);
        await _unitOfWork.Commit();
        await SendAll("UserConnected", user.Id.ToString());
    }

    public async Task OnDisconnected()
    {
        var user = await _unitOfWork.GetReadRepository<AppUser, int>().GetByIdAsync(_helper.GetUserIdByToken());
        user.ConnectionId = null;
        user.IsOnline = false;
        user.LastActivityDate = DateTime.UtcNow.ToLocalTime().ToString("HH:mm:ss") + " PM";
        await _unitOfWork.Commit();
        await SendAll("UserDisconnected", user.Id.ToString());
    }


    public async Task OnLogOut(string userId) =>
        await SendAll("UserDisconnected", userId);

    private async Task SendAll(string method, string param) =>
            await hub.Clients.All.SendAsync(method, param);



    public async Task SendMessage(MessageDTO model, string connectionId)
    {
        if (connectionId is not null)
            await hub.Clients.Client(connectionId).SendAsync("ReceivedMessage", model);
    }

    public async Task OnChangedMessageState(string focusedConnectionId, string currentConnectionId, int userId)
    {
        if (focusedConnectionId is not null)
        {
            await hub.Clients.Client(focusedConnectionId).SendAsync("OnUserFocusConnectedToMe", userId);
            await hub.Clients.AllExcept(focusedConnectionId, currentConnectionId).SendAsync("OnUserFocusDisconnectedToMe", userId);
        }
        else
            await hub.Clients.AllExcept(currentConnectionId).SendAsync("OnUserFocusDisconnectedToMe", userId);

    }
    public async Task OnHasChanges(string connectionId, int userId) =>
            await hub.Clients.All.SendAsync("OnHasChanges", userId);
    //await hub.Clients.AllExcept(connectionId).SendAsync("OnHasChanges", userId);
}

