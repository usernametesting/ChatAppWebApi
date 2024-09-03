using Application.Abstractions.Services;
using Application.DTOs.AuthDTOs;
using Application.DTOs.Tokens;
using ChatApis.Helpers;
using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ChatApis.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly HttpResult _httpResult;

    public AccountController(IAuthService authService, SignInManager<AppUser> signInManager, HttpResult checker)
    {
        _authService = authService;
        _signInManager = signInManager;
        _httpResult = checker;
    }



    [HttpPost("Register")]
    public async Task<IActionResult> Register(CreateUserDTO model)
    {
        var result = await _authService.RegisterAsync(model);
        return await _httpResult.Result(result);
    }


    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginUserDTO model)
    {
        var result = await _authService.LoginAsync(model);
        return await _httpResult.Result(result);
    }


    [HttpGet("ConfirmGmail")]
    public async Task<IActionResult> ConfirmGmail(int id, string token)
    {
        var result = await _authService.ConfirmEmailAsync(id, token);
        return await _httpResult.Result(result);
    }
    [HttpGet("Logout")]

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok();
    }
    [HttpPost("RefreshToken")]
    public async Task<IActionResult> RefreshToken( [FromBody]RefreshTokenDTO refreshToken)
    {
        var result = await _authService.RefrehTokenAsync(refreshToken);
        return await _httpResult.Result(result);
    }

    [HttpGet("AccessDenied")]
    public IActionResult AccessDenied() =>
    Unauthorized("You do not have permission");

}
