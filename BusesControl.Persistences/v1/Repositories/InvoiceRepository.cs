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

    public async Task<InvoiceModel?> GetByIdWithFinancialAsync(Guid id)
    {
        return await _context.Invoices.AsNoTracking().Include(x => x.Financial).SingleOrDefaultAsync(x => x.Id == id);
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
