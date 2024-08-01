using BusesControl.Entities.Models;
using BusesControl.Persistence.Contexts;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Persistence.v1.Repositories;

public class InvoiceExpenseRepository(
    AppDbContext context
) : IInvoiceExpenseRepository
{
    private readonly AppDbContext _context = context;

    public async Task<IEnumerable<InvoiceExpenseModel>> FindByFinancialAsync(Guid financialId)
    {
        return await _context.InvoicesExpense.Where(x => x.FinancialId == financialId).ToListAsync();
    }

    public async Task<InvoiceExpenseModel?> GetByIdAsync(Guid id)
    {
        return await _context.InvoicesExpense.SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<InvoiceExpenseModel?> GetByExternalAsync(string external)
    {
        return await _context.InvoicesExpense.SingleOrDefaultAsync(x => x.ExternalId == external);
    }

    public async Task<InvoiceExpenseModel?> GetByIdAndExternalAsync(Guid id, string external)
    {
        return await _context.InvoicesExpense.SingleOrDefaultAsync(x => x.Id == id && x.ExternalId == external);
    }

    public async Task<bool> CreateAsync(InvoiceExpenseModel record)
    {
        await _context.InvoicesExpense.AddAsync(record);
        return true;
    }

    public bool Update(InvoiceExpenseModel record)
    {
        _context.InvoicesExpense.Update(record);
        return true;
    }

    public bool UpdateRange(IEnumerable<InvoiceExpenseModel> records)
    {
        _context.InvoicesExpense.UpdateRange(records);
        return true;
    }

    public async Task<bool> ExistsByReferenceAsync(string reference)
    {
        return await _context.InvoicesExpense.AnyAsync(x => x.Reference == reference);
    }
}
