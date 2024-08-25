using Application.Abstractions.Services.ControllerServices;
using Application.DTOs.AuthDTOs;
using ChatApis.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace ChatApis.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly HttpResult _httpResult;

    public UserController(IUserService userService, HttpResult checker)
    {
        _userService = userService;
        _httpResult = checker;
    }


    public async Task<IActionResult> GetAll()
    {
        var result = (await _userService.GetAllAsync());
        return await _httpResult.Result(result);
    }

    public async Task<IActionResult> Delete(int Id)
    {
        var result = await _userService.Delete(Id);
        return await _httpResult.Result(await _userService.Delete(Id));
    }

    public async Task<IActionResult> Edit(int Id)
    {
        var result = await _userService.GetByIdAsync(Id);
        return await _httpResult.Result(result);
    }
    [HttpPost]
    public async Task<IActionResult> Edit(UpdateUserDTO model)
    {
        var result = await _userService.Update(model);
        return await _httpResult.Result(result);
    }

    [HttpGet]
    public async Task<IActionResult> Recover(int Id)
    {
        var result = await _userService.Recover(Id);
        return await _httpResult.Result(result);
    }
}
