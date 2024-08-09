using BusesControl.Entities.Enums.v1;
using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Requests.v1;
using BusesControl.Entities.Responses.v1;

namespace BusesControl.Services.v1.Interfaces;

public interface INotificationService
{
    Task<PaginationResponse<NotificationModel>> GetAllForAdminAsync(PaginationRequest request);
    Task<PaginationResponse<NotificationModel>> FindMyNotificationsAsync(PaginationRequest request);
    Task<NotificationModel> GetByIdAsync(Guid id);
    Task<bool> SendInternalNotificationAsync(string title, string message, NotificationAccessLevelEnum accessLevel);
    Task<bool> CreateAsync(NotificationCreateRequest request, bool system = false);
    Task<bool> DeleteAsync(Guid id);
}
