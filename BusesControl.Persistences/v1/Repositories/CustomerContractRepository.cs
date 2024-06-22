using BusesControl.Entities.Models;
using BusesControl.Persistence.Contexts;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using System.Data.Entity;

namespace BusesControl.Persistence.v1.Repositories;

public class CustomerContractRepository(
    AppDbContext context
) : ICustomerContractRepository
{
    private readonly AppDbContext _context = context;

    public async Task<CustomerContractModel> GetByIdAsync(Guid id)
    {
        return await _context.CustomersContract.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> CreateAsync(CustomerContractModel record)
    {
        await _context.CustomersContract.AddAsync(record);
        return true;
    }

    public bool Update(CustomerContractModel record)
    {
        _context.CustomersContract.Update(record);
        return true;
    }

    public bool Delete(CustomerContractModel record)
    {
        _context.CustomersContract.Remove(record);
        return true;
    }
}
