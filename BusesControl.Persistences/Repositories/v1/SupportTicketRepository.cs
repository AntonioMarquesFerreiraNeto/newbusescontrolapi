using BusesControl.Entities.Enums.v1;
using BusesControl.Entities.Models.v1;
using BusesControl.Persistence.Contexts;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Persistence.Repositories.v1;

public class SupportTicketRepository(
    AppDbContext context
) : GenericRepository<SupportTicketModel>(context), ISupportTicketRepository
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

    public async Task<bool> ExistsByReferenceAsync(string reference)
    {
        return await _context.SupportTickets.AnyAsync(x => x.Reference == reference);
    }
}
