using Application.Abstractions.HubServices;
using Application.Abstractions.Services;
using Application.Abstractions.Services.ExternalServices;
using Application.Abstractions.Token;
using Application.DTOs;
using Application.DTOs.AuthDTOs;
using Application.DTOs.Common;
using Application.DTOs.ExternalServiceDTOs;
using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Persistence.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Implementions.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IMailService _mailService;
    private readonly IAutoMapperConfiguration _mapper;
    private readonly ProductDbContext productDbContext;
    private readonly INotificationHubService _notifyService;
    private readonly ITokenHandler _tokenHandler;

    public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IMailService mailService, IAutoMapperConfiguration mapper, ProductDbContext productDbContext, INotificationHubService notifyService, ITokenHandler tokenHandler)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _mailService = mailService;
        _mapper = mapper;
        this.productDbContext = productDbContext;
        _notifyService = notifyService;
        _tokenHandler = tokenHandler;
    }



    public async Task<ServiceResult<Token>> LoginAsync(LoginUserDTO model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
            return new ()
            {
                StatusCode = HttpStatusCode.BadRequest,
                Message = "user not found",
                Success = false,
            };

        if (!user.EmailConfirmed)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            await _mailService.SendMail(new SendMailDTO()
            {
                Email = user.Email,
                CallBackUrl = $"http://localhost:5205/Account/ConfirmGmail?id={user.Id}&token={WebUtility.UrlEncode(token)}"
            });

            return new ()
            {
                StatusCode = HttpStatusCode.FailedDependency,
                Message = "account was unconfirmed,check your email to confirm account",
                Success = false,
            };
        }
        var result = await _signInManager.PasswordSignInAsync(user, model.Password, isPersistent: false, lockoutOnFailure: false);
        if (!result.Succeeded)
        {
            return new ()
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Message = "Invalid password",
                Success = false,
            };
        }
        await _notifyService.SendNotification("AdminNotfications", $"{user.UserName} logged") ;
        return new ()
        {
            StatusCode = HttpStatusCode.OK,
            Message = "OK",
            Success = true,
            resultObj = _tokenHandler.CreateAccessToken(3600,user),
        };


    }

    public async Task<ServiceResult> RegisterAsync(CreateUserDTO model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user is not null)
            return new ()
            {
                StatusCode = HttpStatusCode.AlreadyReported,
                Success = false,
                Message = "user already exsists"
            };
        user = _mapper.Map<AppUser, CreateUserDTO>(model);
        user.UserName = model.Email.Substring(0, model.Email.IndexOf('@'));
        var result = await _userManager.CreateAsync(user, model.Password);
        await _userManager.AddToRoleAsync(user, "User");
        if (result.Succeeded)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);


            await _mailService.SendMail(new SendMailDTO()
            {
                Email = model.Email,
                CallBackUrl = $"http://localhost:5205/Account/ConfirmGmail?id={user.Id}&token={WebUtility.UrlEncode(token)}"
            });

            await _notifyService.SendNotification("AdminNotfications", $"{user.UserName} registered");
            
            return new ()
            {
                Message = "user succsessfully created and check your email to confirm your account",
                StatusCode = HttpStatusCode.Created,
                Success = true,
            };
        }

        return new ()
        {
            Message = string.Join(", ", result.Errors.Select(e => e.Description)),
            StatusCode = HttpStatusCode.BadRequest,
            Success = result.Succeeded,
        };

    }

    public async Task<ServiceResult> ConfirmEmailAsync(int id, string token)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        var result = await _userManager.ConfirmEmailAsync(user, token);

        if (result.Succeeded)
            return new ()
            {
                Message = "confirmation was successfully",
                StatusCode = HttpStatusCode.OK,
                Success = result.Succeeded,
            };
        return new ()
        {
            Message = string.Join(",", result.Errors.Select(e => e.Description)),
            StatusCode = HttpStatusCode.BadRequest,
            Success = result.Succeeded,
        };
    }

  
}
