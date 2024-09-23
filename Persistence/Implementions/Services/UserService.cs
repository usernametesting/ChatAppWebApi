using Application.Abstractions.Services;
using Application.Abstractions.Services.ControllerServices;
using Application.Abstractions.Services.ExternalServices;
using Application.DTOs.AuthDTOs;
using Application.DTOs.Common;
using Application.DTOs.SignalRDTOs;
using Application.DTOs.Statuses;
using Application.DTOs.UsersDTOs;
using Application.Helpers.Users;
using Application.Repositories.Users;
using Application.UnitOfWorks;
using Domain.Entities.ConcretEntities;
using Domain.Enums.MessageEnums;
using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SignalR.Implementions.Services;
using System.Collections;
using System.Data;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;


namespace Persistence.Implementions.Services;
public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGCService _gcService;
    private readonly IUserHelper _helper;
    private readonly IAutoMapperConfiguration _mapper;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly IUserReadrepository<AppUser, int> userRepo;




    public UserService(IUserHelper helper, IUnitOfWork unitOfWork, IAutoMapperConfiguration mapper, RoleManager<AppRole> roleManager, UserManager<AppUser> userManager, IUserReadrepository<AppUser, int> userRepo, IGCService gcService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _roleManager = roleManager;
        _userManager = userManager;
        this.userRepo = userRepo;
        _helper = helper;
        _gcService = gcService;
    }

    public async Task<ServiceResult> AddContactAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
            return new ServiceResult
            {
                StatusCode = HttpStatusCode.OK,
                Success = false,
                Message = "user does't exsists"
            };

        var currentlyUser = await _unitOfWork.GetReadRepository<AppUser, int>().GetByIdAsync(_helper.GetCurrentlyUserId());

        currentlyUser?.Contacts?.Add(new()
        {
            ContactUser = user,
        });

        await _unitOfWork.Commit();

        return new ServiceResult
        {
            StatusCode = HttpStatusCode.Created,
            Success = true,
            Message = "contact succsessfully added"
        };

    }

    public async Task<ServiceResult> Delete(int Id)
    {
        try
        {
            var repo = _unitOfWork.GetWriteRepository<AppUser, int>();
            await repo.DeleteById(Id);
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

    public async Task<ServiceResult<CurrentlyUser>> GetCurrentlyUserAsync()
    {
        var currenltyUserId = _helper.GetCurrentlyUserId();
        var user = await _unitOfWork.GetReadRepository<AppUser, int>().GetByIdAsync(currenltyUserId);

        return new ServiceResult<CurrentlyUser>
        {
            Success = true,
            Message = "succsess",
            StatusCode = HttpStatusCode.OK,
            resultObj = _mapper.Map<CurrentlyUser, AppUser>(user)
        };
    }

    public async Task<ServiceResult<UserDTO>> GetUserByIdAsync(int userId)
    {
        var senderId = _helper.GetCurrentlyUserId();
        var messages = await userRepo.GetUserByIdAsync(int.Parse(senderId.ToString()), userId);

        return new ServiceResult<UserDTO>
        {
            Success = true,
            Message = "succsess",
            StatusCode = HttpStatusCode.OK,
        };
    }

    public async Task<ServiceResult<List<UserDTO>>> GetUsersWithMessagesAsync()
    {
        var senderId = _helper.GetCurrentlyUserId();
        var messages = await userRepo.GetUsersWithMessages(int.Parse(senderId.ToString()));

        return new ServiceResult<List<UserDTO>>
        {
            Success = true,
            Message = "succsess",
            StatusCode = HttpStatusCode.OK,
            resultObj = messages
        };
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

    public async Task<ServiceResult<string>> UpdateMessageStateOnConnectedAsync(string connectionId)
    {
        var result = new ServiceResult<string>();
        try
        {
            var a = _helper.GetUserIdByToken();
            var user = await _unitOfWork.GetReadRepository<AppUser, int>().GetByIdAsync(_helper.GetUserIdByToken());
            user.ConnectionId = connectionId;
            user.IsOnline = true;

            var updateValues = new Dictionary<string, object>
            {
                { "State", MessageState.NOTIFIED},
            };

            await _unitOfWork.GetWriteRepository<Message, int>()
                           .UpdateMultipleAsync(updateValues, m => m.UserMessages.ToUserId == user.Id
                                                && m.UserMessages.Message.State != MessageState.SEEN);

            await _unitOfWork.Commit();
            result.Success = true;
            result.Message = "succsess";
            result.StatusCode = HttpStatusCode.OK;
            result.resultObj = user.Id.ToString();
        }
        catch (Exception e)
        {
            result.Success = false;
            result.Message = e.InnerException?.Message ?? e.Message;
            result.StatusCode = HttpStatusCode.BadRequest;
        }


        return result;
    }

    public async Task<ServiceResult<string>> UpdateUserStateOnDisconnectAsync()
    {
        var user = await _unitOfWork.GetReadRepository<AppUser, int>().GetByIdAsync(_helper.GetUserIdByToken());
        user.ConnectionId = null;
        user.IsOnline = false;

        user.LastActivityDate = DateTime.UtcNow.ToLocalTime().ToString("HH:mm:ss") + " PM";
        await _unitOfWork.Commit();
        return new ServiceResult<string>()
        {
            Success = true,
            Message = "succsess",
            StatusCode = HttpStatusCode.OK,
            resultObj = user.Id.ToString()
        };
    }
  
}
