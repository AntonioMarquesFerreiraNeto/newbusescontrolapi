using BusesControl.Entities.Models;
using BusesControl.Persistence.Contexts;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Persistence.v1.Repositories;

public class ContractBusReplacementRepository(
    AppDbContext context
) : IContractBusReplacementRepository
{
    private readonly AppDbContext _context = context;

    public async Task<IEnumerable<ContractBusReplacementModel>> FindByContractAsync(Guid contractId)
    {
        var query = _context.ContractBusReplacements.AsNoTracking();

        query = query.Include(x => x.Bus);

        query = query.Where(x => x.ContractId == contractId);

        return await query.ToListAsync();
    }

    public async Task<ContractBusReplacementModel?> GetByIdAsync(Guid id)
    {
        return await _context.ContractBusReplacements.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ContractBusReplacementModel?> GetByIdAndContractAsync(Guid id, Guid contractId)
    {
        return await _context.ContractBusReplacements.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id && x.ContractId == contractId);
    }

    public async Task<ContractBusReplacementModel?> GetByIdAndContractWithBusAsync(Guid id, Guid contractId)
    {
        return await _context.ContractBusReplacements.AsNoTracking().Include(x => x.Bus).SingleOrDefaultAsync(x => x.Id == id && x.ContractId == contractId);
    }

    public async Task<bool> CreateAsync(ContractBusReplacementModel record)
    {
        await _context.ContractBusReplacements.AddAsync(record);
        return true;
    }

    public bool Update(ContractBusReplacementModel record)
    {
        _context.ContractBusReplacements.Update(record);
        return true;
    }

    public bool Delete(ContractBusReplacementModel record)
    {
        _context.ContractBusReplacements.Remove(record);
        return true;
    }
}
