using Application.Abstractions.Services;
using Application.Abstractions.Services.ControllerServices;
using Application.Abstractions.Services.InternalServices;
using Application.DTOs.AuthDTOs;
using Application.DTOs.Common;
using Application.UnitOfWorks;
using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Data;
using System.Net;


namespace Persistence.Implementions.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDeepCopy _deepCopy;
    private readonly IAutoMapperConfiguration _mapper;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly UserManager<AppUser> _userManager;


    public UserService(IUnitOfWork unitOfWork, IDeepCopy deepCopy, IAutoMapperConfiguration mapper, RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _deepCopy = deepCopy;
        _mapper = mapper;
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task<ServiceResult> Delete(int Id)
    {
        try
        {
            await _unitOfWork.GetWriteRepository<AppUser, int>().DeleteById(Id);
            await _unitOfWork.Commit();
            return new()
            {
                StatusCode = HttpStatusCode.OK,
                Message = "user was succsessfully deleted",
                Success = true
            };
        }
        catch (Exception e)
        {
            return new()
            {
                StatusCode = HttpStatusCode.BadRequest,
                Message = e.InnerException?.Message,
                Success = false
            };
        }


    }

    public async Task<ServiceResult<List<AppUser>>> GetAllAsync()
    {
        try
        {
            var users = (await _unitOfWork.GetReadRepository<AppUser, int>().GetAllAsQueryableAsync());
            return new()
            {
                Message = "request was succsessfully",
                Success = true,
                StatusCode = HttpStatusCode.OK,
                resultObj = users.IgnoreQueryFilters().ToList()
            };
        }
        catch (Exception e)
        {
            return new()
            {
                Message = e.InnerException?.Message,
                Success = false,
                StatusCode = HttpStatusCode.BadRequest,
            };
        }
    }

    public Task<ServiceResult<List<AppUser>>> GetAllDataAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResult<UpdateUserDTO>> GetByIdAsync(int id)
    {

        try
        {
            var user = await _unitOfWork.GetReadRepository<AppUser, int>().GetByIdAsync(id);
            if (user is null)
                return new()
                {
                    Message = "Dəcəlliy eləmə",
                    Success = false,
                    StatusCode = HttpStatusCode.BadRequest,
                };

            var mappedUser = _mapper.Map<UpdateUserDTO, AppUser>(user);
            mappedUser.RoleNames = user.UserRoles?.Select(role => role.Role.Name).ToList()!;
            mappedUser.AllRoles = await _roleManager.Roles.ToListAsync();

            return new()
            {
                Message = "request was succsessfully",
                Success = true,
                StatusCode = HttpStatusCode.OK,
                resultObj = mappedUser
            };
        }
        catch (Exception e)
        {
            return new()
            {
                Message = e.InnerException?.Message,
                Success = false,
                StatusCode = HttpStatusCode.BadRequest,
            };
        }
    }

    public async Task<ServiceResult> Recover(int Id)
    {
        var user = await (await _unitOfWork.GetReadRepository<AppUser, int>().GetAllAsQueryableAsync()).
                                    IgnoreQueryFilters().FirstOrDefaultAsync(user => user.Id == Id);
        if (user is null)
            return new ServiceResult
            {
                Success = false,
                Message = "User not found",
                StatusCode = HttpStatusCode.NotFound
            };
        user.IsDeleted = !user.IsDeleted;
        try
        {
            await _unitOfWork.Commit();
        }
        catch (Exception e)
        {
            return new ServiceResult
            {
                Success = false,
                Message = e?.InnerException?.Message! ?? "user already exsists",
                StatusCode = HttpStatusCode.BadRequest
            };
        }
        return new ServiceResult
        {
            Success = true,
            Message = "User was recovered",
            StatusCode = HttpStatusCode.OK
        };
    }

    public async Task<ServiceResult> Update(UpdateUserDTO model)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(model.Id.ToString());
            if (user == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "User not found",
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRolesAsync(user, model.RoleNames!);

            foreach (var prop in model.GetType().GetProperties())
            {
                var result = user?.GetType().GetProperty(prop.Name);
                if (result != null)
                {
                    var entityPropertyType = result.PropertyType;

                    if (typeof(IEnumerable).IsAssignableFrom(entityPropertyType) && entityPropertyType != typeof(string))
                    {
                        var entityList = result.GetValue(user) as IList;

                        if (entityList != null && prop.GetValue(model) is IEnumerable modelList)
                            foreach (var item in modelList)
                                entityList.Add(item);
                    }
                    else
                        result.SetValue(user, prop.GetValue(model));
                }
            }
            var updateResult = await _userManager.UpdateAsync(user!);
            if (updateResult.Succeeded)
                return new()
                {
                    StatusCode = HttpStatusCode.OK,
                    Success = updateResult.Succeeded,
                    Message = "User was succsessfully updated",
                };
            return new()
            {
                StatusCode = HttpStatusCode.BadRequest,
                Success = updateResult.Succeeded,
                Message = string.Join(',', updateResult.Errors),
            };

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

    }


}
