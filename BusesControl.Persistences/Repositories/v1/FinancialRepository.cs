using BusesControl.Entities.DTOs;
using BusesControl.Entities.Enums.v1;
using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Responses.v1;
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
        var query = _context.Financials.Include(x => x.Customer).Include(x => x.Supplier).OrderByDescending(x => x.CreatedAt).AsNoTracking();

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

    public async Task<IEnumerable<FinancialModel>> FindRecentsByQuantities(int quantities)
    {
        return await _context.Financials
            .Include(x => x.Customer)
            .Include(x => x.Supplier)
            .OrderByDescending(x => x.CreatedAt)
            .AsNoTracking()
            .Take(quantities).ToListAsync();
    }

    public async Task<int> CountBySearchAsync(string? search = null)
    {
        var query = _context.Financials.Include(x => x.Customer).Include(x => x.Supplier).AsNoTracking();

        if (search is not null)
        {
            query = query.Where(x => x.Customer!.Name.Contains(search) || x.Supplier!.Name.Contains(search));
        }

        return await query.CountAsync();
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
                .Include(x => x.SettingPanel).ThenInclude(y => y!.Requester)
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

    public async Task<IEnumerable<FinancialComparativeResponse>> GetYearlyComparativeAsync()
    {
        var query = _context.Financials.Where(x => x.CreatedAt.Year == DateTime.Now.Year && x.Active);

        return await query.GroupBy(x => new { x.CreatedAt.Month, x.Type }).Select(x => new FinancialComparativeResponse 
        { 
            Month = x.Key.Month,
            TotalValuePeriod = x.Sum(x => x.TotalPrice),
            FinancialType = x.Key.Type
        }).ToListAsync();
    }

    public async Task<FinancialBalanceResponse> GetBalanceAsync()
    {
        var query = _context.Financials.Where(x => x.Active && x.CreatedAt.Year == DateTime.Now.Year);
        var records = await query.Select(x => new FinancialResumeDto { TotalPrice = x.TotalPrice, Type = x.Type }).ToListAsync();

        var response = new FinancialBalanceResponse 
        { 
            ExpenseTotal = records.Where(x => x.Type == FinancialTypeEnum.Expense).Sum(x => x.TotalPrice),
            RevenueTotal = records.Where(x => x.Type == FinancialTypeEnum.Revenue).Sum(x => x.TotalPrice)
        };

        response.Balance = response.RevenueTotal - response.ExpenseTotal;

        return response;
    }
}
