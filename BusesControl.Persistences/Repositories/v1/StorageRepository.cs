using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BusesControl.Entities.Responses.v1;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BusesControl.Persistence.Repositories.v1;

public class StorageRepository : IStorageRepository
{
    public StorageRepository(IOptions<AppSettings> options, ILogger<StorageRepository> logger)
    {
        _settings = options.Value;
        _logger = logger;
    }

    private readonly AppSettings _settings;
    private readonly ILogger _logger;

    public async Task<FileResponse> GetFileAsync(string containerName, string fileName)
    {
        try
        {
            var blobService = new BlobServiceClient(_settings.Azure.Storage.ConnectionString);

            var containerClient = blobService.GetBlobContainerClient(containerName);

            var blobClient = containerClient.GetBlobClient(fileName);
            var blobContent = await blobClient.DownloadContentAsync();

            return new FileResponse
            {
                FileName = fileName,
                ContentType = blobContent.Value.Details.ContentType,
                FileContent = blobContent.Value.Content.ToArray(),
            };
        }
        catch(Exception ex)
        {
            _logger.LogError(ex.Message, "Erro ao obter arquivo {FileName} no container {Container}", fileName, containerName);
            return default!;
        }
    }

    public async Task<string> UploadAysnc(string fileName, string contentType, byte[] file)
    {
        var blobService = new BlobServiceClient(_settings.Azure.Storage.ConnectionString);

        var containerClient = blobService.GetBlobContainerClient(_settings.Azure.Storage.ContainerName);
        await containerClient.CreateIfNotExistsAsync();

        var blobClient = containerClient.GetBlobClient(fileName);

        using var stream = new MemoryStream(file);

        var options = new BlobUploadOptions
        {
            HttpHeaders = new BlobHttpHeaders { ContentType = contentType }
        };

        await blobClient.UploadAsync(stream, options);

        return fileName;
    }

    public async Task<bool> RemoveAsync(string containerName, string fileName)
    {
        try
        {
            var blobService = new BlobServiceClient(_settings.Azure.Storage.ConnectionString);

            var containerClient = blobService.GetBlobContainerClient(containerName);
            var resultRemove = await containerClient.DeleteBlobIfExistsAsync(fileName);

            return resultRemove;
        }
        catch (Exception ex) 
        {
            _logger.LogError(ex.Message, "Erro ao obter arquivo {FileName} no container {Container}", fileName, containerName);
            return false;
        } 
    }
}
