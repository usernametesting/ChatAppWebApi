using Application.Abstractions.Services;
using Application.Abstractions.Services.ControllerServices;
using Application.Abstractions.Services.InternalServices;
using Application.DTOs.Common;
using Application.DTOs.RoleDTOs;
using Application.UnitOfWorks;
using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace Persistence.Implementions.Services;

public class RoleService : IRoleService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDeepCopy _deepCopy;
    private readonly IAutoMapperConfiguration _mapper;
    private readonly RoleManager<AppRole> _roleManager;


    public RoleService(IUnitOfWork unitOfWork, IDeepCopy deepCopy, IAutoMapperConfiguration mapper, RoleManager<AppRole> roleManager)
    {
        _unitOfWork = unitOfWork;
        _deepCopy = deepCopy;
        _mapper = mapper;
        _roleManager = roleManager;
    }

    public async Task<ServiceResult> Add(AddRoleDTO model)
    {
        var role = _mapper.Map<AppRole, AddRoleDTO>(model);
        role.NormalizedName = model.Name.ToUpper();
        await _unitOfWork.GetWriteRepository<AppRole, int>().AddAsync(role);
        await _unitOfWork.Commit();
        return new()
        {
            Message = "role was succsessfully added",
            StatusCode = HttpStatusCode.Created,
            Success = true,
        };
    }

    public async Task<ServiceResult> Delete(int Id)
    {
        var role = await _roleManager.FindByIdAsync(Id.ToString());
        var result = await _roleManager.DeleteAsync(role!);
        if (result.Succeeded)
            return new()
            {
                Success = result.Succeeded,
                Message = string.Join(',', result.Errors),
                StatusCode = HttpStatusCode.BadRequest,
            };
        return new()
        {
            Success = result.Succeeded,
            Message = "Role was succsessfully deleted",
            StatusCode = HttpStatusCode.OK,
        };
    }
    public async Task<ServiceResult<List<AppRole>>> GetAllAsync()
    {
        try
        {
            var roles = await _unitOfWork.GetReadRepository<AppRole, int>().GetAllAsync();
            return new()
            {
                Message = "request was succsess",
                StatusCode = HttpStatusCode.OK,
                Success = true,
                resultObj = roles.ToList()
            };
        }
        catch (Exception e)
        {
            return new()
            {
                Message = e.InnerException?.Message,
                StatusCode = HttpStatusCode.BadRequest,
                Success = false,
            };
        }
    }


    public async Task<ServiceResult> Update(int Id, UpdateRoleDTO model)
    {
        try
        {
            var role = await _unitOfWork.GetReadRepository<AppRole, int>().GetByIdAsync(Id);
            var mappedRole = _mapper.Map<AppRole, UpdateRoleDTO>(model);
            _deepCopy.Copy(mappedRole, role);
            await _unitOfWork.Commit();
        }
        catch (Exception e)
        {
            return new()
            {
                StatusCode = HttpStatusCode.BadRequest,
                Success = false,
                Message = e.InnerException?.Message
            };
        }
        return new()
        {
            StatusCode = HttpStatusCode.OK,
            Success = true,
            Message = "Role was succsessfully updated"
        };
    }



    public async Task<ServiceResult<AppRole>> GetByIdAsync(int id)
    {
        try
        {
            var role = await _unitOfWork.GetReadRepository<AppRole, int>().GetByIdAsync(id);
            var mappedRole = _mapper.Map<AppRole, AppRole>(role);
            return new()
            {
                StatusCode = HttpStatusCode.OK,
                Success = true,
                Message = "request was succsessfully",
                resultObj = mappedRole
            };
        }
        catch (Exception e)
        {
            return new()
            {
                StatusCode = HttpStatusCode.BadRequest,
                Success = false,
                Message = e.InnerException?.Message,
            };

        }

    }

    
}
