using BusesControl.Entities.Models;
using BusesControl.Persistence.Contexts;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Persistence.v1.Repositories;

public class CustomerContractRepository(
    AppDbContext context
) : ICustomerContractRepository
{
    private readonly AppDbContext _context = context;

    public async Task<CustomerContractModel?> GetByIdAsync(Guid id)
    {
        return await _context.CustomersContract.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<CustomerContractModel?> GetByContractAndCustomerWithIncludesAsync(Guid contractId, Guid customerId)
    {
        return await _context.CustomersContract.AsNoTracking()
                        .Include(x => x.Contract).ThenInclude(x => x.Approver)
                        .Include(x => x.Contract).ThenInclude(x => x.Driver)
                        .Include(x => x.Contract).ThenInclude(x => x.SettingsPanel)
                        .Include(x => x.Contract).ThenInclude(x => x.Bus.Color)
                        .Include(x => x.Customer)
                        .SingleOrDefaultAsync(x => x.ContractId == contractId && x.CustomerId == customerId);
    }

    public async Task<IEnumerable<CustomerContractModel>> FindByContractAsync(Guid contractId)
    {
        return await _context.CustomersContract.AsNoTracking().Where(x => x.ContractId == contractId).ToListAsync();
    }

    public async Task<bool> CreateRangeAsync(IEnumerable<CustomerContractModel> record)
    {
        await _context.CustomersContract.AddRangeAsync(record);
        return true;
    }

    public bool Update(CustomerContractModel record)
    {
        _context.CustomersContract.Update(record);
        return true;
    }

    public bool RemoveRange(IEnumerable<CustomerContractModel> records)
    {
        _context.CustomersContract.RemoveRange(records);
        return true;
    }

    public async Task<int> CountByContractAsync(Guid contractId)
    {
        return await _context.CustomersContract.AsNoTracking().Where(x => x.ContractId == contractId).CountAsync();
    }
}
