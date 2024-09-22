using Application.Abstractions.Services.ControllerServices;
using Application.Abstractions.Services.ExternalServices;
using Application.Abstractions.Services.SignalRServices;
using Application.DTOs.Common;
using Application.Helpers.Users;
using Application.UnitOfWorks;
using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace Persistence.Implementions.Services;

public class FileUploadService : IFileUploadService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGCService _gcService;
    private readonly IUserHelper _helper;
    private readonly IHubConnectionsHandler hubConnectionsHandler;


    public FileUploadService(IUnitOfWork unitOfWork, IGCService gcService, IUserHelper helper, IHubConnectionsHandler hubConnectionsHandler)
    {
        _unitOfWork = unitOfWork;
        _gcService = gcService;
        _helper = helper;
        this.hubConnectionsHandler = hubConnectionsHandler;
    }

    public async Task<ServiceResult<string>> ChangeUserImageAsync(IFormFile formFile)
    {
        var url = await _gcService.UploadFileAsync(formFile, formFile.FileName);

        var currentlyUser = await _unitOfWork.GetReadRepository<AppUser, int>().GetByIdAsync(_helper.GetCurrentlyUserId());

        currentlyUser.ProfImageUrl = url;

        await _unitOfWork.Commit();

        await hubConnectionsHandler.OnHasChanges(currentlyUser.ConnectionId!,currentlyUser.Id);

        return new ServiceResult<string>
        {
            Success = true,
            Message = "succsess",
            StatusCode = HttpStatusCode.OK
        };
    }
}
