using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Responses.v1;
using BusesControl.Persistence.Contexts;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Persistence.Repositories.v1;

public class ExportRepository(
    AppDbContext context
) : GenericRepository<ExportModel>(context), IExportRepository
{
    private readonly AppDbContext _context = context;

    public async Task<PaginationResponse<ExportModel>> GetPaginatedAsync(int page, int pageSize) 
    {
        return new PaginationResponse<ExportModel> 
        {
            Response = await _context.Exports.OrderByDescending(x => x.CreatedAt).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(),
            TotalSize = _context.Exports.Count(),
        };
    }

    public async Task<IEnumerable<ExportModel>> GetExpiredsAsync()
        => await _context.Exports.Where(x => DateTime.Now >= x.ExpiresAt).ToListAsync();
}
