using BusesControl.Entities.Models;

namespace BusesControl.Persistence.v1.Repositories.Interfaces;

public interface INotificationRepository
{
    Task<IEnumerable<NotificationModel>> FindForAdminAsync(int page = 0, int pageSize = 0);
    Task<IEnumerable<NotificationModel>> FindMyNotificationsAsync(Guid employeeId, string role, int page = 0, int pageSize = 0);
    Task<NotificationModel?> GetByIdAsync(Guid id);
    Task<bool> CreateAsync(NotificationModel record);
    bool Delete(NotificationModel record);
}
