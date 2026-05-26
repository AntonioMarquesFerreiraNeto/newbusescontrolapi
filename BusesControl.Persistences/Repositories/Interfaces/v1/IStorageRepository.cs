using BusesControl.Entities.Responses.v1;

namespace BusesControl.Persistence.Repositories.Interfaces.v1
{
    public interface IStorageRepository
    {
        Task<FileResponse> GetFileAsync(string containerName, string fileName);
        Task<string> UploadAysnc(string fileName, string contentType, byte[] file);
        Task<bool> RemoveAsync(string containerName, string fileName);
    }
}
