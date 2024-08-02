using BusesControl.Entities.Models.v1;
using BusesControl.Persistence.Contexts;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Persistence.Repositories.v1;

public class CustomerContractRepository(
    AppDbContext context
) : GenericRepository<CustomerContractModel>(context), ICustomerContractRepository
{
    private readonly AppDbContext _context = context;

    public async Task<CustomerContractModel?> GetByIdAsync(Guid id)
    {
        return await _context.CustomersContract.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<CustomerContractModel?> GetByContractAndCustomerAsync(Guid contractId, Guid customerId)
    {
        return await _context.CustomersContract.AsNoTracking().SingleOrDefaultAsync(x => x.ContractId == contractId && x.CustomerId == customerId);
    }

    public async Task<CustomerContractModel?> GetByContractAndCustomerWithIncludesAsync(Guid contractId, Guid customerId)
    {
        return await _context.CustomersContract.AsNoTracking()
                        .Include(x => x.Contract).ThenInclude(x => x.Approver)
                        .Include(x => x.Contract).ThenInclude(x => x.Driver)
                        .Include(x => x.Contract).ThenInclude(x => x.SettingPanel)
                        .Include(x => x.Contract).ThenInclude(x => x.ContractDescription)
                        .Include(x => x.Contract).ThenInclude(x => x.Bus.Color)
                        .Include(x => x.Customer)
                        .SingleOrDefaultAsync(x => x.ContractId == contractId && x.CustomerId == customerId);
    }

    public async Task<IEnumerable<CustomerContractModel>> FindByContractAsync(Guid contractId)
    {
        return await _context.CustomersContract.AsNoTracking().Where(x => x.ContractId == contractId).ToListAsync();
    }

    public async Task<IEnumerable<CustomerContractModel>> FindByProcessTerminationAsync(bool processTermination)
    {
        return await _context.CustomersContract.AsNoTracking().Where(x => x.ProcessTermination == processTermination && x.Active == true).ToListAsync();
    }

    public async Task<int> CountByContractAsync(Guid contractId)
    {
        return await _context.CustomersContract.AsNoTracking().Where(x => x.ContractId == contractId).CountAsync();
    }
}
