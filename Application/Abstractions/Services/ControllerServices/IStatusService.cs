using Application.DTOs.Common;
using Microsoft.AspNetCore.Http;

namespace Application.Abstractions.Services.ControllerServices;

public  interface IStatusService
{
    Task<ServiceResult> PostStatus(IFormFile file, string message);
    Task<ServiceResult> DeleteStatusAsync(int id);

}
