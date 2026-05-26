using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Responses.v1;

namespace BusesControl.Persistence.Repositories.Interfaces.v1;

public interface IExportRepository : IGenericRepository<ExportModel>
{
    Task<PaginationResponse<ExportModel>> GetPaginatedAsync(int page, int pageSize);
    Task<IEnumerable<ExportModel>> GetExpiredsAsync();
}
