using BusesControl.Entities.Models.v1;
using BusesControl.Persistence.Contexts;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Persistence.Repositories.v1;

public class InvoiceExpenseRepository(
    AppDbContext context
) : GenericRepository<InvoiceExpenseModel>(context), IInvoiceExpenseRepository
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

    public async Task<bool> ExistsByReferenceAsync(string reference)
    {
        return await _context.InvoicesExpense.AnyAsync(x => x.Reference == reference);
    }
}
