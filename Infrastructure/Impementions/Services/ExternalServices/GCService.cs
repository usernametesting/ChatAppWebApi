
using Application.Abstractions.Services.ExternalServices;
using Application.ConfigurationsMapping;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;

namespace ExampleForGoogleCloud.Services;

public class GCService : IGCService
{

    private readonly GCSetting _gcConfigs;
    private readonly GoogleCredential _googleCredential;
    StorageClient storageClient = null;

    public GCService(IOptions<GCSetting> options)
    {
        _gcConfigs = options.Value;
        _googleCredential = GoogleCredential.FromJson(File.ReadAllText(_gcConfigs.SettingsPath!));
        storageClient = StorageClient.Create(_googleCredential);
    }

    public async Task DeleteFileAsync(string fileName)
    {
        await storageClient.DeleteObjectAsync(_gcConfigs.BucketName, fileName);
    }

    [Obsolete]
    public async Task<string> GetFileAsync(string fileName, int timeOut = 6000)
    {
        var ServicCredential = _googleCredential.UnderlyingCredential as ServiceAccountCredential;

        var urlSigner = UrlSigner.FromServiceAccountCredential(ServicCredential);

        string url;
        try
        {
            url = await urlSigner.SignAsync(_gcConfigs.BucketName, fileName, TimeSpan.FromSeconds(timeOut));

        }
        catch (Exception e)
        {

            throw;
        }

        return url;
    }

    public async Task<string> UploadFileAsync(IFormFile file, string fileName)
    {
        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        var obj = await storageClient.UploadObjectAsync(_gcConfigs.BucketName, fileName, file.ContentType, memoryStream);


        var url = await GetFileAsync(fileName);
        return url;
    }
}
