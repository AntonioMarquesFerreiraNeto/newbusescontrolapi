using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons.Message;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BusesControl.Business.v1;

public class BusBusiness(
    INotificationApi _notificationApi,
    IBusRepository _busRepository 
) : IBusBusiness
{
    public async Task<bool> ExistsByRenavamOrLicensePlateOrChassisAsync(string renavam, string licensePlate, string chassi, Guid? id = null)
    {
        var exists = await _busRepository.ExistsByRenavamOrLicensePlateOrChassisAsync(renavam, licensePlate, chassi, id);
        if (exists)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status409Conflict,
                title: NotificationTitle.Conflict,
                details: SupportMessage.Bus.Exists
            );
            return false;
        }

        return true;
    }
}
