using BusesControl.Entities.Enums;
using BusesControl.Entities.Models;
using BusesControl.Persistence.Contexts;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Persistence.v1.Repositories;

public class NotificationRepository(
    AppDbContext context
) : INotificationRepository
{
    private readonly AppDbContext _context = context;

    public async Task<IEnumerable<NotificationModel>> FindForAdminAsync(int page = 0, int pageSize = 0)
    {
        return await _context.Notifications.Skip((page - 1) * pageSize).Take(pageSize).AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<NotificationModel>> FindMyNotificationsAsync(Guid employeeId, string role, int page = 0, int pageSize = 0)
    {
        var query = _context.Notifications.AsNoTracking();

        if (role == "Admin")
        {
            query = query.Where(x => x.AccessLevel == NotificationAccessLevelEnum.Admin || x.AccessLevel == NotificationAccessLevelEnum.Public);
        }
        else
        {
            query = query.Where(x => x.AccessLevel == NotificationAccessLevelEnum.Assistant || x.AccessLevel == NotificationAccessLevelEnum.Public);
        }

        query = query.Skip((page - 1) * pageSize).Take(pageSize);

        return await query.ToListAsync();
    }

    public async Task<NotificationModel?> GetByIdAsync(Guid id)
    {
        return await _context.Notifications.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> CreateAsync(NotificationModel record)
    {
        await _context.Notifications.AddAsync(record);
        return true;
    }

    public bool Delete(NotificationModel record)
    {
        _context.Notifications.Remove(record);
        return true;
    }
}
