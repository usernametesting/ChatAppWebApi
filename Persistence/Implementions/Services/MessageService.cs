using Application.Abstractions.Services.ControllerServices;
using Application.Abstractions.Services.ExternalServices;
using Application.Abstractions.Services.SignalRServices;
using Application.DTOs.Common;
using Application.DTOs.SignalRDTOs;
using Application.Helpers.Users;
using Application.UnitOfWorks;
using Domain.Entities.ConcretEntities;
using Domain.Enums.MessageEnums;
using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;
using SignalR.Implementions.Services;
using System.Net;
using System.Reflection.Metadata;
using System.Text.Json;

namespace Persistence.Implementions.Services;

public class MessageService : IMessageService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserHelper _helper;
    private readonly IHubConnectionsHandler hubConnectionsHandler;
    private readonly IGCService _gcService;


    public MessageService(IUnitOfWork unitOfWork, IUserHelper helper, IHubConnectionsHandler hubConnectionsHandler, IGCService gcService)
    {
        _unitOfWork = unitOfWork;
        _helper = helper;
        this.hubConnectionsHandler = hubConnectionsHandler;
        _gcService = gcService;
    }

    public async Task<ServiceResult> PostMessageToUserAsync(MessageDTO model)
    {
        var userMessages = _unitOfWork.GetWriteRepository<UsersMessages, int>();
        await userMessages.AddAsync(new()
        {
            CreatedDate = DateTime.UtcNow,
            FromUserId = _helper.GetCurrentlyUserId(),
            ToUserId = (int)model.ToUserId!,
            Message = new Message()
            {
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                Content = model.Message,
                MessageType = model.MessageType,
                State = model.State

            }
        });

        var toUser = await _unitOfWork.GetReadRepository<AppUser, int>().GetByIdAsync((int)model.ToUserId);
        model.ToUserId = _helper.GetCurrentlyUserId();
        model.IsSender = !model.IsSender;
        await _unitOfWork.Commit();
        await hubConnectionsHandler.SendMessage(model, toUser.ConnectionId!);

        return new ServiceResult()
        {
            Success = true,
            Message = "succsess",
            StatusCode = HttpStatusCode.OK,
        };
    }
    public async Task<ServiceResult> ChangeMessageStateAsync(int toUserId)
    {
        var currentUserId = _helper.GetCurrentlyUserId();
        var currentUser = await _unitOfWork.GetReadRepository<AppUser, int>().GetByIdAsync(currentUserId);
        var focusedUser = await _unitOfWork.GetReadRepository<AppUser, int>().GetByIdAsync(toUserId);

        var updateValues = new Dictionary<string, object>
        {
            { "State", MessageState.SEEN},
        };

        await _unitOfWork.GetWriteRepository<Message, int>()
            .UpdateMultipleAsync(updateValues, m => m.UserMessages.FromUserId == toUserId
            && m.UserMessages.ToUserId == currentUserId);

        currentUser.OnFocusUserId = toUserId;

        await hubConnectionsHandler.OnChangedMessageState(focusedUser.ConnectionId!, currentUser.ConnectionId!, currentUserId);

        return new ServiceResult()
        {
            Success = true,
            StatusCode = HttpStatusCode.OK,
            Message = "succsess",
        };
    }

    public async Task<ServiceResult> PostFile(IFormFile file,string message)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var model = JsonSerializer.Deserialize<MessageDTO>(message, options);
        var url = await _gcService.UploadFileAsync(file, file.FileName);

        var userMessages = _unitOfWork.GetWriteRepository<UsersMessages, int>();
        await userMessages.AddAsync(new()
        {
            CreatedDate = DateTime.UtcNow,
            FromUserId = _helper.GetCurrentlyUserId(),
            ToUserId = (int)model.ToUserId!,
            Message = new Message()
            {
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                Content = url,
                MessageType = model.MessageType,
                State = model.State

            }
        });

        var toUser = await _unitOfWork.GetReadRepository<AppUser, int>().GetByIdAsync((int)model.ToUserId);
        model.ToUserId = _helper.GetCurrentlyUserId();
        model.IsSender = !model.IsSender;
        model.Message = url;
        await _unitOfWork.Commit();
        await hubConnectionsHandler.SendMessage(model, toUser.ConnectionId!);

        return new ServiceResult()
        {
            Success = true,
            Message = "succsess",
            StatusCode = HttpStatusCode.OK,
        };
    }
}
