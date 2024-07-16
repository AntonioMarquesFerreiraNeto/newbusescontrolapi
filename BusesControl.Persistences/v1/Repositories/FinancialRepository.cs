using BusesControl.Entities.Models;
using BusesControl.Persistence.Contexts;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Persistence.v1.Repositories;

public class FinancialRepository(
    AppDbContext context
) : IFinancialRepository
{
    private readonly AppDbContext _context = context;

    public async Task<FinancialModel?> GetByContractAndCustomerWithInvoicesAsync(Guid contractId, Guid customerId)
    {
        return await _context.Financials.Include(x => x.Invoices).AsNoTracking().SingleOrDefaultAsync(x => x.ContractId == contractId && x.CustomerId == customerId);
    }

    public async Task<bool> CreateAsync(FinancialModel record)
    {
        await _context.Financials.AddAsync(record);
        return true;
    }

    public async Task<bool> CreateRangeAsync(IEnumerable<FinancialModel> records)
    {
        await _context.Financials.AddRangeAsync(records);
        return true;
    }

    public bool Update(FinancialModel record)
    {
        _context.Financials.Update(record);
        return true;
    }
}
