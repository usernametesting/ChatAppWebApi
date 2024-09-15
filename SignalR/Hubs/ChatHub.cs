using Application.Abstractions.Services.SignalRServices;
using Application.DTOs.SignalRDTOs;
using Application.UnitOfWorks;
using Domain.BaseModels;
using Domain.Entities.ConcretEntities;
using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using SignalR.Helpers;
using System.Runtime.InteropServices;

namespace SignalR.Hubs;
public class ChatHub : Hub, IChatHub
{

    private readonly IHubConnectionsHandler _handler;

    public ChatHub(IHubConnectionsHandler handler)
    {
        _handler = handler;
    }

    public override async Task OnConnectedAsync()
    {
        await _handler.OnConnected(Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        await _handler.OnDisconnected();
        await base.OnDisconnectedAsync(exception);
    }



}