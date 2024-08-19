using BusesControl.Entities.Enums.v1;
using BusesControl.Entities.Models.v1;
using BusesControl.Persistence.Contexts;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Persistence.Repositories.v1;

public class InvoiceRepository(
    AppDbContext context
) : GenericRepository<InvoiceModel>(context), IInvoiceRepository
{
    private readonly AppDbContext _context = context;

    public async Task<IEnumerable<InvoiceModel>> FindByFinancialAsync(Guid financialId)
    {
        return await _context.Invoices.Where(x => x.FinancialId == financialId).ToListAsync();
    }

    public async Task<IEnumerable<InvoiceModel>> FindByDueDateForSystemAsync(DateOnly date)
    {
        var query = _context.Invoices.Include(x => x.Financial).AsNoTracking();

        query = query.Where(x => x.DueDate == date && x.Status != InvoiceStatusEnum.Paid && x.Status != InvoiceStatusEnum.Canceled);

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<InvoiceModel>> FindByStatusForSystemWithFinancialAsync(InvoiceStatusEnum status)
    {
        var query = _context.Invoices.Include(x => x.Financial.SettingPanel).AsNoTracking();

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

    public async Task<bool> ExistsByReferenceAsync(string reference)
    {
        return await _context.Invoices.AnyAsync(x => x.Reference == reference);
    }
}
