using Application.Abstractions.Services;
using Application.Abstractions.Services.ExternalServices;
using Application.Abstractions.Services.InternalServices;
using Application.Abstractions.Token;
using Infrastructure.Abstractions.Services.Token;
using Infrastructure.Impementions.Helpers;
using Infrastructure.Impementions.Services.ExternalServices;
using Microsoft.Extensions.DependencyInjection;


namespace Infrastructure;

public  static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IAutoMapperConfiguration, AutoMapperConfiguration>();
        services.AddScoped<IAesManager, AesManager>();
        services.AddScoped<ITokenHandler, TokenHandler>();
        services.AddScoped<IMailService, MailService>();
        services.AddScoped<IDeepCopy, DeepCopy>();

        return services;
    }
}
