using Application.Abstractions.HubServices;
using Application.Abstractions.Services;
using Application.Abstractions.Services.ExternalServices;
using Application.Abstractions.Services.SignalRServices;
using Application.DTOs;
using Application.DTOs.AuthDTOs;
using Application.DTOs.Common;
using Application.DTOs.Common.Bases;
using Application.DTOs.ExternalServiceDTOs;
using Application.DTOs.Tokens;
using Application.Helpers.Users;
using Application.UnitOfWorks;
using Domain.Entities.Identity;
using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Persistence.DbContexts;

using System.Net;

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
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHubConnectionsHandler _hubConnectionsHandler;
    private readonly IUserHelper _helper;


    public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IMailService mailService, IAutoMapperConfiguration mapper, ProductDbContext productDbContext, INotificationHubService notifyService, ITokenHandler tokenHandler, IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, IHubConnectionsHandler hubConnectionsHandler, IUserHelper helper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _mailService = mailService;
        _mapper = mapper;
        this.productDbContext = productDbContext;
        _notifyService = notifyService;
        _tokenHandler = tokenHandler;
        _httpContextAccessor = httpContextAccessor;
        _unitOfWork = unitOfWork;
        _hubConnectionsHandler = hubConnectionsHandler;
        _helper = helper;
    }



    public async Task<IServiceResult> LoginAsync(LoginUserDTO model)
    {
        AppUser user = await _userManager?.FindByEmailAsync(model.Email)!;

        if (user == null)
            return new ServiceResult()
            {
                StatusCode = HttpStatusCode.BadRequest,
                Message = "user not found",
                Success = false,
            };

        if (!user.EmailConfirmed)
        {
            await sendEmailConfirmationMail(user);

            return new ServiceResult()
            {
                StatusCode = HttpStatusCode.FailedDependency,
                Message = "account was unconfirmed,check your email to confirm account",
                Success = false,
            };
        }


        if (user.IsOnline)
            return new ServiceResult()
            {
                StatusCode = HttpStatusCode.BadRequest,
                Message = "user already login",
                Success = false,
            };

        var result = await _signInManager.PasswordSignInAsync(user, model.Password, isPersistent: false, lockoutOnFailure: false);
        if (!result.Succeeded)
        {
            return new ServiceResult()
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Invalid password",
                Success = false,
            };
        }
        await _notifyService.SendNotification("AdminNotfications", $"{user.UserName} logged");

        var token = _tokenHandler.CreateAccessToken(user);
        var refreshToken = (await _unitOfWork.GetReadRepository<AppUserToken, int>()
              .GetAllAsQueryableAsync()).FirstOrDefault(token => token.UserId == user.Id);

        if (refreshToken is null)
            await _unitOfWork.GetWriteRepository<AppUserToken, int>().AddAsync(new AppUserToken()
            {
                CreatedDate = DateTime.UtcNow,
                LoginProvider = "Default",
                Expires = DateTime.UtcNow.AddDays(1),
                Name = "RefreshToken",
                Value = token.RefreshToken,
                UserId = user.Id,
            });
        else
        {
            refreshToken.Value = token.RefreshToken;
            refreshToken.Expires = DateTime.UtcNow.AddDays(1);
        }
        await _unitOfWork.Commit();
        await CreateOrUpdateRefreshToken(user, token.RefreshToken);
        return new ServiceResult<Token>()
        {
            StatusCode = HttpStatusCode.OK,
            Message = "Login was succsessfully",
            Success = true,
            resultObj = token
        };


    }

    private async Task CreateOrUpdateRefreshToken(AppUser user, string token)
    {
        var refreshToken = (await _unitOfWork.GetReadRepository<AppUserToken, int>()
                .GetAllAsQueryableAsync()).FirstOrDefault(token => token.UserId == user.Id);

        if (refreshToken is null)
            user?.UserTokens?.Add(new AppUserToken()
            {
                CreatedDate = DateTime.UtcNow,
                LoginProvider = "Default",
                Expires = DateTime.UtcNow.AddDays(1),
                Name = "RefreshToken",
                Value = token,
                User = user
            });
        else
        {
            refreshToken.Value = token;
            refreshToken.Expires = DateTime.UtcNow.AddDays(1);
        }
        await _unitOfWork.Commit();
    }

    private async Task sendEmailConfirmationMail(AppUser user)
    {
        var emailConfirmToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        await _mailService.SendMail(new SendMailDTO()
        {
            Email = user.Email,
            CallBackUrl = $"https://192.168.100.9:7293/api/Account/ConfirmGmail?id={user.Id}&token={WebUtility.UrlEncode(emailConfirmToken)}"
        });
    }



    public async Task<ServiceResult> RegisterAsync(CreateUserDTO model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user is not null)
            return new()
            {
                StatusCode = HttpStatusCode.OK,
                Success = false,
                Message = "user already exsists"
            };
        user = _mapper.Map<AppUser, CreateUserDTO>(model);
        user.UserName = model.Email.Substring(0, model.Email.IndexOf('@'));
        var result = await _userManager.CreateAsync(user, model.Password);
        //await _userManager.AddToRoleAsync(user, "User");
        if (result.Succeeded)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);


            await _mailService.SendMail(new SendMailDTO()
            {
                Email = model.Email,
                CallBackUrl = $"http://localhost:27001/Account/ConfirmGmail?id={user.Id}&token={WebUtility.UrlEncode(token)}"
            });

            await _notifyService.SendNotification("AdminNotfications", $"{user.UserName} registered");

            return new()
            {
                Message = "user succsessfully created and check your email to confirm your account",
                StatusCode = HttpStatusCode.Created,
                Success = true,
            };
        }

        return new()
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
            return new()
            {
                Message = "confirmation was successfully",
                StatusCode = HttpStatusCode.OK,
                Success = result.Succeeded,
            };
        return new()
        {
            Message = string.Join(",", result.Errors.Select(e => e.Description)),
            StatusCode = HttpStatusCode.BadRequest,
            Success = result.Succeeded,
        };
    }
    [Authorize]

    public async Task<IServiceResult> RefrehTokenAsync(RefreshTokenDTO refreshToken)
    {
        var user = await (await _unitOfWork.GetReadRepository<AppUser, int>().GetAllAsQueryableAsync())
                                        .FirstOrDefaultAsync(u => u.UserTokens!.Any(t => t.Value == refreshToken.RefreshToken));

        var token = user?.UserTokens?.FirstOrDefault(t => t.Value == refreshToken.RefreshToken);
        if (token is null)
            return new ServiceResult()
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Message = "Refresh token not found",
                Success = false
            };
        if (token.Expires < DateTime.UtcNow)
            return new ServiceResult()
            {
                StatusCode = HttpStatusCode.NotAcceptable,
                Message = "Refresh token expired",
                Success = false
            };

        var newToken = _tokenHandler.CreateAccessToken(user!);
        await CreateOrUpdateRefreshToken(user, newToken.RefreshToken);
        return new ServiceResult<Token>()
        {
            Message = "user token updated",
            StatusCode = HttpStatusCode.OK,
            Success = true,
            resultObj = newToken
        };

    }

    public async Task<ServiceResult> Logut()
    {
        var user = await _unitOfWork.GetReadRepository<AppUser, int>().GetByIdAsync(_helper.GetCurrentlyUserId());
        user.ConnectionId = null;
        user.IsOnline = false;

        user.LastActivityDate = DateTime.UtcNow.ToLocalTime().ToString("HH:mm:ss") + " PM";
        await _unitOfWork.Commit();

        await _hubConnectionsHandler.OnLogOut(user.Id.ToString());
        return new ServiceResult()
        {
            Success = true,
            Message = "succsess",
            StatusCode = HttpStatusCode.OK,
        };
     

    }
}
