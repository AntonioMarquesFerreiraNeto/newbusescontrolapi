using BusesControl.Entities.Enums.v1;
using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Requests.v1;

namespace BusesControl.Services.v1.Interfaces;

public interface INotificationService
{
    Task<IEnumerable<NotificationModel>> GetAllForAdminAsync(PaginationRequest request);
    Task<IEnumerable<NotificationModel>> FindMyNotificationsAsync(PaginationRequest request);
    Task<NotificationModel> GetByIdAsync(Guid id);
    Task<bool> SendInternalNotificationAsync(string title, string message, NotificationAccessLevelEnum accessLevel);
    Task<bool> CreateAsync(NotificationCreateRequest request, bool system = false);
    Task<bool> DeleteAsync(Guid id);
}
