using Application.DTOs.SignalRDTOs;
using Domain.BaseModels;
using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.SignalR;

namespace Application.Abstractions.Services.SignalRServices;

public interface IHubConnectionsHandler
{
    Task OnConnected(string connectionId);  
    Task OnDisconnected();  
    Task SendMessage(MessageDTO model,string connectionId);
    Task OnChangedMessageState(string connectionId, string currentConnectionId, int userId);
    Task OnHasChanges(string connectionId,int userId);
    Task OnLogOut(string userId);
}
