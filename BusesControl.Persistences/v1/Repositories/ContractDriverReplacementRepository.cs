using BusesControl.Entities.Models;
using BusesControl.Persistence.Contexts;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Persistence.v1.Repositories;

public class ContractDriverReplacementRepository(
    AppDbContext context
) : IContractDriverReplacementRepository
{
    private readonly AppDbContext _context = context;

    public async Task<IEnumerable<ContractDriverReplacementModel>> FindByContractAsync(Guid contractId)
    {
        var query = _context.ContractDriverReplacements.AsNoTracking();

        query = query.Include(x => x.Driver);

        query = query.Where(x => x.ContractId == contractId);

        return await query.ToListAsync();
    }

    public async Task<ContractDriverReplacementModel?> GetByIdAsync(Guid id)
    {
        return await _context.ContractDriverReplacements.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ContractDriverReplacementModel?> GetByIdAndContractAsync(Guid id, Guid contractId)
    {
        return await _context.ContractDriverReplacements.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id && x.ContractId == contractId);
    }

    public async Task<ContractDriverReplacementModel?> GetByIdAndContractWithDriverAsync(Guid id, Guid contractId)
    {
        return await _context.ContractDriverReplacements.AsNoTracking().Include(x => x.Driver).SingleOrDefaultAsync(x => x.Id == id && x.ContractId == contractId);
    }

    public async Task<bool> CreateAsync(ContractDriverReplacementModel record)
    {
        await _context.ContractDriverReplacements.AddAsync(record);
        return true;
    }

    public bool Update(ContractDriverReplacementModel record)
    {
        _context.ContractDriverReplacements.Update(record);
        return true;
    }

    public bool Delete(ContractDriverReplacementModel record)
    {
        _context.ContractDriverReplacements.Remove(record);
        return true;
    }
}
