using Application.DTOs.Common;
using Application.DTOs.SignalRDTOs;

namespace Application.Abstractions.Services.ControllerServices;

public interface IMessageService
{
    Task<ServiceResult> ChangeMessageStateAsync(int userId);
    Task<ServiceResult> PostMessageToUserAsync(MessageDTO model);

}
