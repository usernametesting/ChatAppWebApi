using Application.Abstractions.HubServices;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.Hubs;

public class NotificationHubService : Hub, INotificationHubService
{
    private readonly IHubContext<NotificationHubService>? _hubContext;

    public NotificationHubService(IHubContext<NotificationHubService>? hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task SendNotification(string key, string value)
    {
        await _hubContext!.Clients.All.SendAsync(key, value);
    }
}