using Application.DTOs.Common;
using Application.DTOs.RoleDTOs;
using ETicaretAPI.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions.Services.ControllerServices;

public interface IRoleService
{
    Task<ServiceResult<List<ReturnRoleDTO>>> GetAllAsync();
    Task<ServiceResult<AppRole>> GetByIdAsync(int id);

    Task<ServiceResult> Delete(int Id);

    Task<ServiceResult> Update(int Id, UpdateRoleDTO model);

    Task<ServiceResult> Add(AddRoleDTO model);
}
