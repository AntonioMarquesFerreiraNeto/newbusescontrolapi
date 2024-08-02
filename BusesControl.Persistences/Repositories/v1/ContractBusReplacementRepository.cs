using BusesControl.Entities.Models.v1;
using BusesControl.Persistence.Contexts;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Persistence.Repositories.v1;

public class ContractBusReplacementRepository(
    AppDbContext context
) : GenericRepository<ContractBusReplacementModel>(context), IContractBusReplacementRepository
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
}
