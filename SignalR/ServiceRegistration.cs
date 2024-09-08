using Application.Abstractions.HubServices;
using Application.Abstractions.Services.SignalRServices;
using Microsoft.Extensions.DependencyInjection;
using SignalR.Hubs;
using SignalR.Implementions.Services;


namespace Persistence;

public static class ServiceRegistration
{
    public static IServiceCollection AddSignalRServices(this IServiceCollection services)
    {
        services.AddScoped<INotificationHubService, NotificationHubService>();
        services.AddScoped<IHubConnectionsHandler, HubConnectionsHandler>();
        return services;
    }
}
