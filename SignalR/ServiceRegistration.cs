using Application.Abstractions.HubServices;
using Microsoft.Extensions.DependencyInjection;
using SignalR.Hubs;


namespace Persistence;

public static class ServiceRegistration
{
    public static IServiceCollection AddSignalRServices(this IServiceCollection services)
    {
        services.AddScoped<INotificationHubService, NotificationHubService>();

        return services;
    }
}
