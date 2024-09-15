using Application.Abstractions.Services;
using Application.ConfigurationsMapping;
using ChatApis.Helpers;
using CompositionRoot;
using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Persistence.DbContexts;
using Persistence.IdentityCustoms;


namespace ProductMVC;

public static class ServiceRegistration
{
    public static IServiceCollection AddPresentationService(this IServiceCollection services,IConfiguration configuration)
    {
        //services.AddScoped<UserManager<,>,CustomUserManager<,>>();
        services.Configure<GCSetting>(configuration);
        services.AddHttpContextAccessor();
        services.AddScoped<HttpResult>();
        services.AddApplicationDependencies(configuration);
        services.AddSignalR();

        return services;
    }
}
