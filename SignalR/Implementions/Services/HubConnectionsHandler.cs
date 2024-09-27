using Application.Abstractions.Services.ControllerServices;
using Application.Abstractions.Services.SignalRServices;
using Application.DTOs.Common;
using Application.DTOs.SignalRDTOs;
using Application.UnitOfWorks;
using Domain.BaseModels;
using Domain.Entities.ConcretEntities;
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
    private readonly IUserService _userService;
    private readonly IHubContext<ChatHub> hub;

    public HubConnectionsHandler(IHubContext<ChatHub> hub, IUserService userService)
    {
        this.hub = hub;
        _userService = userService;
    }

    public async Task OnConnected(string connectionId)
    {
        var result = await _userService.UpdateMessageStateOnConnectedAsync(connectionId);
        await SendAll("UserConnected", result.resultObj!);
    }

    public async Task OnDisconnected()
    {
        var result = await _userService.UpdateUserStateOnDisconnectAsync();
        await SendAll("UserDisconnected", result.resultObj!);
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

