using Application.Abstractions.Services;
using Application.Abstractions.Services.ControllerServices;
using Application.DTOs.AuthDTOs;
using Application.DTOs.SignalRDTOs;
using ChatApis.Helpers;
using Domain.Entities.ConcretEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;


namespace ChatApis.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]

public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IFileUploadService _fileUploadService;
    private readonly IMessageService _messageService;
    private readonly HttpResult _httpResult;
    private readonly IAuthService _authService;
    private readonly IStatusService _statusService;



    public UserController(IUserService userService, HttpResult checker, IMessageService messageService, IFileUploadService fileUploadService, IAuthService authService, IStatusService statusService)
    {
        _userService = userService;
        _httpResult = checker;
        _messageService = messageService;
        _fileUploadService = fileUploadService;
        _authService = authService;
        _statusService = statusService;
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> GetAll()
    {
        var A = HttpContext;
        var result = (await _userService.GetAllAsync());
        return await _httpResult.Result(result);
    }
    [HttpGet("[action]")]
    public async Task<IActionResult> GetCurrentlyUser()
    {
        var result = (await _userService.GetCurrentlyUserAsync());
        return await _httpResult.Result(result);
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> GetUserWithMessages()
    {
        var result = await _userService.GetUsersWithMessagesAsync();
        return await _httpResult.Result(result);
    }


    [HttpGet("[action]")]
    public async Task<IActionResult> GetUserById(int userId)
    {
        var result = await _userService.GetUserByIdAsync(userId);
        return await _httpResult.Result(result);
    }
    [HttpDelete("[action]")]
    public async Task<IActionResult> Delete(int Id)
    {
        var result = await _userService.Delete(Id);
        return await _httpResult.Result(result);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Edit(UpdateUserDTO model)
    {
        var result = await _userService.Update(model);
        return await _httpResult.Result(result);
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> Recover(int Id)
    {
        var result = await _userService.Recover(Id);
        return await _httpResult.Result(result);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> ChangedMessageState([FromBody] int userId)
    {
        var result = await _messageService.ChangeMessageStateAsync(userId);
        return await _httpResult.Result(result);
    }
    [HttpPost("[action]")]
    public async Task<IActionResult> PostMessage(MessageDTO model)
    {
        var result = await _messageService.PostMessageToUserAsync(model);
        return await _httpResult.Result(result);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> PostFile([FromForm] IFormFile file, [FromForm] string message)
    {
        var result = await _messageService.PostFile(file, message);
        return await _httpResult.Result(result);
    }

   

    [HttpPost("PostStatus")]
    public async Task<IActionResult> PostStatus([FromForm] IFormFile file, [FromForm] string status)
    {
        var result = await _statusService.PostStatus(file, status);
        return await _httpResult.Result(result);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> ChangeUserImage([FromForm] IFormFile formFile)
    {
        var result = await _fileUploadService.ChangeUserImageAsync(formFile);
        return await _httpResult.Result(result);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> AddContact([FromBody] string email)
    {
        var result = await _userService.AddContactAsync(email);
        return await _httpResult.Result(result);
    }
    [HttpPost("[action]")]
    public async Task<IActionResult> DeleteContact([FromBody] int id)
    {
        var result = await _userService.DeleteContactAsync(id);
        return await _httpResult.Result(result);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Logout()
    {
        var result = await _authService.Logut();
        return await _httpResult.Result(result);
    }
    
    [HttpPost("[action]")]
    public async Task<IActionResult> UpdateUserBio([FromBody]string bio)
    {
        var result = await _userService.UpdateUserBioAsync(bio);
        return await _httpResult.Result(result);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> DeleteStatus([FromBody] int id)
    {
        var result = await _statusService.DeleteStatusAsync(id);
        return await _httpResult.Result(result);
    }
}
