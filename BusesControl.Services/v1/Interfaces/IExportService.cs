using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Requests.v1;
using BusesControl.Entities.Responses.v1;

namespace BusesControl.Services.v1.Interfaces
{
    public interface IExportService
    {
        Task<PaginationResponse<ExportModel>> GetPaginatedAsync(PaginationRequest request);
        Task<FileResponse> GetFileAsync(string fileName);
        Task<bool> CreateAsync(ExportCreateRequest request);
        Task<bool> RemoveExpireds();
    }
}
