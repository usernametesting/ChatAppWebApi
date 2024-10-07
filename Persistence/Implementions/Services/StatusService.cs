using Application.Abstractions.Services.ControllerServices;
using Application.Abstractions.Services.ExternalServices;
using Application.Abstractions.Services.SignalRServices;
using Application.DTOs.Common;
using Application.DTOs.Statuses;
using Application.Helpers.Users;
using Application.UnitOfWorks;
using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace Persistence.Implementions.Services;

public class StatusService : IStatusService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGCService _gcService;
    private readonly IUserHelper _helper;
    private readonly IHubConnectionsHandler hubConnectionsHandler;


    public StatusService(IUnitOfWork unitOfWork, IGCService gcService, IUserHelper helper, IHubConnectionsHandler hubConnectionsHandler)
    {
        _unitOfWork = unitOfWork;
        _gcService = gcService;
        _helper = helper;
        this.hubConnectionsHandler = hubConnectionsHandler;
    }

    public async Task<ServiceResult> DeleteStatusAsync(int id)
    {
        var currentlyUser = await _unitOfWork.GetReadRepository<AppUser, int>().GetByIdAsync(_helper.GetCurrentlyUserId());
        currentlyUser.Statuses!.Remove(currentlyUser.Statuses.FirstOrDefault(s => s.Id == id)!);
        await _unitOfWork.Commit();

        return new ServiceResult
        {
            StatusCode = HttpStatusCode.OK,
            Success = true,
            Message = "contact succsessfully deleted"
        };
    }

    public async Task<ServiceResult> PostStatus(IFormFile file, string status)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var model = JsonSerializer.Deserialize<StatusDTO>(status, options);
        var url = await _gcService.UploadFileAsync(file, file.FileName);

        var currentlyUser = await _unitOfWork.GetReadRepository<AppUser, int>().GetByIdAsync(_helper.GetCurrentlyUserId());
        currentlyUser?.Statuses?.Add(new()
        {
            StatusType = model!.StatusType,
            MediaUrl = url,
        });

        await _unitOfWork.Commit();
        await hubConnectionsHandler.OnHasChanges(currentlyUser!.ConnectionId!, currentlyUser.Id);

        return new ServiceResult()
        {
            Success = true,
            Message = "succsess",
            StatusCode = HttpStatusCode.OK,
        };
    }
}
