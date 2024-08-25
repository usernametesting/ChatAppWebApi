using Application.Abstractions.Services;
using Application.Abstractions.Services.Authentications;
using Application.Abstractions.Services.ControllerServices;
using Application.Abstractions.Services.InternalServices;
using Application.Repositories.Commons;
using Application.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.DbContexts;
using Persistence.Implementions.Services;
using Persistence.UnitOfWorks;
using System;


namespace Persistence;

public static class ServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<DbContext, ProductDbContext>();
        services.AddDbContext<ProductDbContext>(op =>
                    op.UseSqlServer(configuration.GetConnectionString("default"))
                    .UseLazyLoadingProxies());

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();

        return services;
    }
}
