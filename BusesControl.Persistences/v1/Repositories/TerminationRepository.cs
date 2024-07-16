using BusesControl.Entities.Models;
using BusesControl.Persistence.Contexts;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Persistence.v1.Repositories;

public class TerminationRepository(
    AppDbContext context
) : ITerminationRepository
{
    private readonly AppDbContext _context = context;

    public async Task<IEnumerable<TerminationModel>> FindByContractAsync(Guid contractId, string? search = null)
    {
        var query = _context.Terminations.Include(x => x.Customer).AsNoTracking();

        query = query.Where(x => x.ContractId == contractId);

        if (search is not null)
        {
            query = query.Where(x => x.Customer.Name.Contains(search));
        }

        return await query.ToListAsync();
    }

    public async Task<TerminationModel?> GetByIdAsync(Guid? id)
    {
        return await _context.Terminations.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> CreateAsync(TerminationModel record)
    {
        await _context.Terminations.AddAsync(record);
        return true;
    }

    public bool Update(TerminationModel record)
    {
        _context.Terminations.Update(record);
        return true;
    }

    public async Task<bool> ExistsByContractAndCustomerAsync(Guid contractId, Guid customerId)
    {
        return await _context.Terminations.AnyAsync(x => x.ContractId == contractId && x.CustomerId == customerId);
    }
}
