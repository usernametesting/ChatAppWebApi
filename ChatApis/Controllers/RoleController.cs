using Application.Abstractions.Services.ControllerServices;
using Application.DTOs.RoleDTOs;
using ChatApis.Helpers;
using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatApis.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;
    private readonly HttpResult _httpResult;

    public RoleController(IRoleService roleService, HttpResult checker)
    {
        _roleService = roleService;
        _httpResult = checker;
    }
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _roleService.GetAllAsync();
        return await _httpResult.Result(result);
    }

    [HttpPost("Add")]

    public async Task<IActionResult> Add(AddRoleDTO model)
    {
        var result = await _roleService.Add(model);
        return await _httpResult.Result(result);
    }
    
    [HttpPost("Edit")]
    public async Task<IActionResult> Edit(UpdateRoleDTO model)
    {
        var result = await _roleService.Update(model.Id, model);
        return await _httpResult.Result(result);
    }
    [HttpDelete("Delete")]
    public async Task<IActionResult> Delete(int Id)
    {
        var result = await _roleService.Delete(Id);
        return await _httpResult.Result(result);
    }
}
