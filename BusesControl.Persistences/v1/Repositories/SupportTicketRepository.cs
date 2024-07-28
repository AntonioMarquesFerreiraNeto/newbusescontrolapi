using BusesControl.Entities.Enums;
using BusesControl.Entities.Models;
using BusesControl.Persistence.Contexts;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Persistence.v1.Repositories;

public class SupportTicketRepository(
    AppDbContext context
) : ISupportTicketRepository
{
    private readonly AppDbContext _context = context;

    public async Task<SupportTicketModel?> GetByIdOptionalEmployeeAsync(Guid id, Guid? employeeId = null)
    {
        return await _context.SupportTickets.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id && (!employeeId.HasValue || x.EmployeeId == employeeId));
    }

    public async Task<SupportTicketModel?> GetByIdOptionalEmployeeWithIncludesAsync(Guid id, Guid? employeeId = null)
    {
        return await _context.SupportTickets.AsNoTracking()
            .Include(x => x.SupportTicketMessages).ThenInclude(x => x.SupportAgent)
            .Include(x => x.SupportAgent)
            .Include(x => x.Employee)
            .SingleOrDefaultAsync(x => x.Id == id && (!employeeId.HasValue || x.EmployeeId == employeeId));
    }

    public async Task<IEnumerable<SupportTicketModel>> FindByStatusAsync(Guid? employeeId = null, SupportTicketStatusEnum? status = null, int page = 0, int pageSize = 0)
    {
        var query = _context.SupportTickets.Include(x => x.Employee).Include(x => x.SupportAgent).AsNoTracking();

        query = query.OrderByDescending(x => x.CreatedAt);

        if (employeeId is not null)
        {
            query = query.Where(x => x.EmployeeId == employeeId);
        }

        if (status is not null)
        {
            query = query.Where(x => x.Status == status);
        }

        if (page > 0 || pageSize > 0)
        {
            query = query.Skip((page - 1) * pageSize).Take(pageSize);    
        }

        return await query.ToListAsync();
    }

    public async Task<bool> CreateAsync(SupportTicketModel record)
    {
        await _context.SupportTickets.AddAsync(record);
        return true;
    }

    public bool Update(SupportTicketModel record)
    {
        _context.SupportTickets.Update(record);
        return true;
    }

    public async Task<bool> ExistsByReferenceAsync(string reference)
    {
        return await _context.SupportTickets.AnyAsync(x => x.Reference == reference);
    }
}
