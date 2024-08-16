using BusesControl.Entities.Enums.v1;
using BusesControl.Entities.Models.v1;
using BusesControl.Persistence.Contexts;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Persistence.Repositories.v1;

public class NotificationRepository(
    AppDbContext context
) : GenericRepository<NotificationModel>(context), INotificationRepository
{
    private readonly AppDbContext _context = context;

    public async Task<IEnumerable<NotificationModel>> GetAllAsync(int page = 0, int pageSize = 0)
    {
        var query = _context.Notifications.Include(x => x.Sender).AsNoTracking();

        query = query.OrderByDescending(x => x.CreatedAt);

        if (page > 0 && pageSize > 0)
        {
            query = query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<NotificationModel>> FindMyNotificationsAsync(string role, int page = 0, int pageSize = 0)
    {
        var query = _context.Notifications.Include(x => x.Sender).AsNoTracking();

        if (role == "Admin")
        {
            query = query.Where(x => x.AccessLevel == NotificationAccessLevelEnum.Admin || x.AccessLevel == NotificationAccessLevelEnum.Public);
        }
        else
        {
            query = query.Where(x => x.AccessLevel == NotificationAccessLevelEnum.Assistant || x.AccessLevel == NotificationAccessLevelEnum.Public);
        }

        query = query.OrderByDescending(x => x.CreatedAt);

        if (page > 0 & pageSize > 0)
        {
            query = query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        return await query.ToListAsync();
    }

    public async Task<int> CountByOptionRoleAsync(string? role = null)
    {
        var query = _context.Notifications.AsNoTracking();

        if (role == "Admin")
        {
            query = query.Where(x => x.AccessLevel == NotificationAccessLevelEnum.Admin || x.AccessLevel == NotificationAccessLevelEnum.Public);
        }
        else if (role == "Assistant")
        {
            query = query.Where(x => x.AccessLevel == NotificationAccessLevelEnum.Assistant || x.AccessLevel == NotificationAccessLevelEnum.Public);
        }

        return await query.CountAsync();
    }

    public async Task<NotificationModel?> GetByIdAsync(Guid id)
    {
        return await _context.Notifications.AsNoTracking().Include(x => x.Sender).SingleOrDefaultAsync(x => x.Id == id);
    }
}
