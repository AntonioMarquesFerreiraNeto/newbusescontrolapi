using BusesControl.Entities.Models.v1;
using BusesControl.Persistence.Contexts;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Persistence.Repositories.v1;

public class FinancialRepository(
    AppDbContext context
) : GenericRepository<FinancialModel>(context), IFinancialRepository
{
    private readonly AppDbContext _context = context;

    public async Task<IEnumerable<FinancialModel>> GetAllAsync()
    {
        return await _context.Financials.AsNoTracking()
                .Include(x => x.Customer)
                .Include(x => x.Supplier)
                .Include(x => x.Invoices)
                .Include(x => x.InvoiceExpenses)
                .ToListAsync();
    }

    public async Task<IEnumerable<FinancialModel>> FindBySearchAsync(int page = 0, int pageSize = 0, string? search = null)
    {
        var query = _context.Financials.Include(x => x.Customer).Include(x => x.Supplier).AsNoTracking();

        if (search is not null)
        {
            query = query.Where(x => x.Customer!.Name.Contains(search) || x.Supplier!.Name.Contains(search));
        }

        if (page > 0 && pageSize > 0)
        {
            query = query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        return await query.ToListAsync();
    }

    public async Task<FinancialModel?> GetByIdAsync(Guid id)
    {
        return await _context.Financials.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<FinancialModel?> GetByIdWithInvoicesAsync(Guid id)
    {
        return await _context.Financials.AsNoTracking().Include(x => x.Invoices).SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<FinancialModel?> GetByIdWithInvoicesExpenseAsync(Guid id)
    {
        return await _context.Financials.AsNoTracking().Include(x => x.InvoiceExpenses).SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<FinancialModel?> GetByContractAndCustomerWithInvoicesAsync(Guid contractId, Guid customerId)
    {
        return await _context.Financials.Include(x => x.Invoices).AsNoTracking().SingleOrDefaultAsync(x => x.ContractId == contractId && x.CustomerId == customerId);
    }

    public async Task<FinancialModel?> GetByIdWithIncludesAsync(Guid id)
    {
        return await _context.Financials.AsNoTracking()
                .Include(x => x.SettingPanel)
                .Include(x => x.Customer)
                .Include(x => x.Supplier)
                .Include(x => x.Invoices)
                .Include(x => x.InvoiceExpenses)
                .SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> ExistsByReferenceAsync(string reference)
    {
        return await _context.Financials.AnyAsync(x => x.Reference == reference);
    }

    public async Task<bool> ExistsBySettingPanelAsync(Guid settingPanelId)
    {
        return await _context.Financials.AnyAsync(x => x.SettingPanelId == settingPanelId);
    }
}
