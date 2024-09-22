using Application.DTOs.Common;
using Application.DTOs.SignalRDTOs;
using Microsoft.AspNetCore.Http;

namespace Application.Abstractions.Services.ControllerServices;

public interface IMessageService
{
    Task<ServiceResult> ChangeMessageStateAsync(int userId);
    Task<ServiceResult> PostMessageToUserAsync(MessageDTO model);
    Task<ServiceResult> PostFile(IFormFile file,string message);

}
