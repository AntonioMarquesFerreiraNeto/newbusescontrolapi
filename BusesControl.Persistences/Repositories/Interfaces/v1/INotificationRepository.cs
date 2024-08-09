using BusesControl.Entities.Models.v1;

namespace BusesControl.Persistence.Repositories.Interfaces.v1;

public interface INotificationRepository : IGenericRepository<NotificationModel>
{
    Task<IEnumerable<NotificationModel>> GetAllAsync(int page = 0, int pageSize = 0);
    Task<IEnumerable<NotificationModel>> FindMyNotificationsAsync(string role, int page = 0, int pageSize = 0);
    Task<int> CountByOptionRoleAsync(string? role = null);
    Task<NotificationModel?> GetByIdAsync(Guid id);
}
