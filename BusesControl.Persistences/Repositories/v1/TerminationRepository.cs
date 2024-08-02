using BusesControl.Entities.Models.v1;
using BusesControl.Persistence.Contexts;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Persistence.Repositories.v1;

public class TerminationRepository(
    AppDbContext context
) : GenericRepository<TerminationModel>(context), ITerminationRepository
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

    public async Task<bool> ExistsByContractAndCustomerAsync(Guid contractId, Guid customerId)
    {
        return await _context.Terminations.AnyAsync(x => x.ContractId == contractId && x.CustomerId == customerId);
    }
}
