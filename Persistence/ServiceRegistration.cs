﻿using Application.Abstractions.Services;
using Application.Abstractions.Services.Authentications;
using Application.Abstractions.Services.ControllerServices;
using Application.Abstractions.Services.InternalServices;
using Application.Helpers.Users;
using Application.Repositories.Commons;
using Application.Repositories.Products;
using Application.Repositories.Users;
using Application.UnitOfWorks;
using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.DbContexts;
using Persistence.Helpers;
using Persistence.IdentityCustoms;
using Persistence.Implementions.Services;
using Persistence.Repositories.Concrets.Messages;
using Persistence.Repositories.Concrets.Users;
using Persistence.UnitOfWorks;
using System;


namespace Persistence;

public static class ServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 23));
        services.AddScoped<UserManager<AppUser>, CustomUserManager<AppUser>>();

        services.AddIdentity<AppUser, AppRole>()
          .AddUserManager<CustomUserManager<AppUser>>()
          .AddEntityFrameworkStores<ProductDbContext>()
          .AddDefaultTokenProviders();

        services.AddScoped<DbContext, ProductDbContext>();
        //services.AddDbContext<ProductDbContext>(op =>
        //            op.UseMySql(configuration.GetConnectionString("default"), serverVersion
        //            //mySqlOptions => mySqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null)
        //            )
        //            .UseLazyLoadingProxies());

        var st = configuration.GetConnectionString("default");

        services.AddDbContext<ProductDbContext>(op =>
                   op.UseSqlServer(configuration.GetConnectionString("default"))
                   .UseLazyLoadingProxies());

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserHelper, UserHelper>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IMessageService, MessageService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IFileUploadService,FileUploadService>();
        services.AddScoped<IUserReadrepository<AppUser, int>, UserReadRepository>();
        services.AddScoped<IStatusService,StatusService>();


        services.Configure<IdentityOptions>(options =>
        {
            options.User.RequireUniqueEmail = false;
            options.User.AllowedUserNameCharacters = null;

            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 1;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredUniqueChars = 1;
        });
        return services;
    }
}
