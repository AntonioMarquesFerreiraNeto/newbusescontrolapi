using BusesControl.Entities.Models;
using BusesControl.Persistence.Contexts;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Persistence.v1.Repositories;

public class ContractDescriptionRepository(
    AppDbContext context
) : IContractDescriptionRepository
{
    private readonly AppDbContext _context = context;

    public async Task<ContractDescriptionModel?> GetByIdAsync(Guid id)
    {
        return await _context.ContractDescriptions.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
    }

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

    public async Task<bool> CreateAsync(ContractDescriptionModel record)
    {
        await _context.ContractDescriptions.AddAsync(record);
        return true;
    }

    public bool Update(ContractDescriptionModel record)
    {
        _context.ContractDescriptions.Update(record);
        return true;
    }

    public bool Delete(ContractDescriptionModel record)
    {
        _context.ContractDescriptions.Remove(record);
        return true;
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
