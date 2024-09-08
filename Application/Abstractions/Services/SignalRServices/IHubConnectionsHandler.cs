using Application.DTOs.SignalRDTOs;
using Domain.BaseModels;
using Microsoft.AspNetCore.SignalR;

namespace Application.Abstractions.Services.SignalRServices;

public interface IHubConnectionsHandler
{
    Task OnConnected();  
    Task OnDisconnected();  
    Task SendMessage(MessageDTO model);  
}
