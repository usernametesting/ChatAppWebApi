using Application.DTOs.AuthDTOs;
using Application.DTOs.Common;
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

    Task<ServiceResult> Delete(int Id);
    Task<ServiceResult> Recover(int Id);
    Task<ServiceResult> Update(UpdateUserDTO model);


}
