using Application.DTOs.AuthDTOs;
using Application.DTOs.Common;
using Application.DTOs.SignalRDTOs;
using Application.DTOs.UsersDTOs;
using Domain.Entities.ConcretEntities;
using ETicaretAPI.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions.Services.ControllerServices;

public interface IUserService
{
    Task<ServiceResult<List<AppUser>>> GetAllAsync();
    Task<ServiceResult<List<AppUser>>> GetAllDataAsync ();
    Task<ServiceResult<UpdateUserDTO>> GetByIdAsync(int id);
    Task<ServiceResult<UserWithMessages>> GetCurrentlyUserAsync();
    Task<ServiceResult<List<UserWithMessages>>> GetUsersWithMessagesAsync();

    Task<ServiceResult> Delete(int Id);
    Task<ServiceResult> Recover(int Id);
    Task<ServiceResult> Update(UpdateUserDTO model);


}
