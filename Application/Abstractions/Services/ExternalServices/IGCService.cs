using Microsoft.AspNetCore.Http;

namespace Application.Abstractions.Services.ExternalServices;

public interface IGCService
{
    Task DeleteFileAsync(string fileName);
    Task<string> GetFileAsync(string fileName, int timeOut = 600);
    Task<string> UploadFileAsync(IFormFile file, string fileName);

}
