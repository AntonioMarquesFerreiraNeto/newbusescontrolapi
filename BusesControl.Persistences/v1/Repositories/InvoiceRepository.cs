using BusesControl.Entities.Enums;
using BusesControl.Entities.Models;
using BusesControl.Persistence.Contexts;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Persistence.v1.Repositories;

public class InvoiceRepository(
    AppDbContext context
) : IInvoiceRepository
{
    private readonly AppDbContext _context = context;

    public async Task<IEnumerable<InvoiceModel>> FindByDueDateForSystemAsync(DateOnly date, bool expenseOnly)
    {
        var query = _context.Invoices.Include(x => x.Financial).AsNoTracking();
        
        query = query.Where(x => x.DueDate == date && x.Status != InvoiceStatusEnum.Paid && x.Status != InvoiceStatusEnum.Canceled);
        
        if (expenseOnly)
        {
            query = query.Where(x => x.Financial.Type == FinancialTypeEnum.Expense);
        }

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<InvoiceModel>> FindByStatusForSystemAsync(InvoiceStatusEnum status)
    {
        var query = _context.Invoices.AsNoTracking();

        query = query.Where(x => x.Status == status && x.DueDate <= DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-2)));

        return await query.ToListAsync();
    }

    public async Task<InvoiceModel?> GetByIdWithFinancialAsync(Guid id)
    {
        return await _context.Invoices.AsNoTracking().Include(x => x.Financial).SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<InvoiceModel?> GetByIdAndExternalAsync(Guid id, string externalId)
    {
        return await _context.Invoices.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id && x.ExternalId == externalId);
    }

    public async Task<bool> CreateAsync(InvoiceModel record)
    {
        await _context.Invoices.AddAsync(record);
        return true;
    }

    public bool Update(InvoiceModel record)
    {
        _context.Invoices.Update(record);
        return true;
    }

    public async Task<bool> ExistsByReferenceAsync(string reference)
    {
        return await _context.Invoices.AnyAsync(x => x.Reference == reference);
    }
}
