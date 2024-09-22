using Application.Abstractions.Services.ExternalServices;
using Application.DTOs.Common;
using Application.UnitOfWorks;
using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace Application.Abstractions.Services.ControllerServices;

public interface IFileUploadService
{
    public Task<ServiceResult<string>> ChangeUserImageAsync(IFormFile formFile);
}
