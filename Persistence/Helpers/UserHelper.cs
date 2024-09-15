
using Application.Helpers.Users;
using Microsoft.AspNetCore.Http;
using SignalR.Helpers;
using System.Security.Claims;

namespace Persistence.Helpers;

public class UserHelper : IUserHelper
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserHelper(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int GetCurrentlyUserId()=>int.Parse(_httpContextAccessor?
            .HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);


    public int GetUserIdByToken()
    {
        var token = _httpContextAccessor.HttpContext.Request.Query["access_token"];
        var userId = JwtDecryptor.Decrypt(token!);
        return int.Parse(userId!);
    }
}
