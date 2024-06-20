using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BusesControl.Business.v1;

public class ColorBusiness(
    INotificationApi _notificationApi,
    IColorRepository _colorRepository
) : IColorBusiness
{
    public async Task<bool> ValidateActiveAsync(Guid id)
    {
        var record = await _colorRepository.GetByIdAsync(id);
        if (record is null) 
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Color.NotFound
            );
            return false;
        }

        if (record.Active is not true)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Color.NotActive
            );
            return false;
        }

        return true;
    }
}
