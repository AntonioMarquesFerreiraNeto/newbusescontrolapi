using BusesControl.Entities.Models.v1;
using BusesControl.Persistence.Contexts;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Persistence.Repositories.v1;

public class ContractDescriptionRepository(
    AppDbContext context
) : GenericRepository<ContractDescriptionModel>(context), IContractDescriptionRepository
{
    private readonly AppDbContext _context = context;

    public async Task<IEnumerable<ContractDescriptionModel>> FindAsync(int page = 0, int pageSize = 0)
    {
        var query = _context.ContractDescriptions.AsNoTracking();

        if (page > 0 & pageSize > 0)
        {
            query = query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        var records = await query.ToListAsync();

        return records;
    }

    public async Task<int> CountAsync()
    {
        return await _context.ContractDescriptions.CountAsync();
    }

    public async Task<ContractDescriptionModel?> GetByIdAsync(Guid id)
    {
        return await _context.ContractDescriptions.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> ExitsAsync(Guid id)
    {
        return await _context.ContractDescriptions.AnyAsync(x => x.Id == id);
    }

    public async Task<bool> ExitsByReferenceAsync(string reference)
    {
        return await _context.ContractDescriptions.AnyAsync(x => x.Reference == reference);
    }
}
