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

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var result = (await _userService.GetAllAsync());
        return await _httpResult.Result(result);
    }
    [HttpDelete("Delete")]
    public async Task<IActionResult> Delete(int Id)
    {
        var result = await _userService.Delete(Id);
        return await _httpResult.Result(result);
    }
  
    [HttpPost("Edit")]
    public async Task<IActionResult> Edit(UpdateUserDTO model)
    {
        var result = await _userService.Update(model);
        return await _httpResult.Result(result);
    }

    [HttpGet("Recover")]
    public async Task<IActionResult> Recover(int Id)
    {
        var result = await _userService.Recover(Id);
        return await _httpResult.Result(result);
    }
}
