using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Enums.v1;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using Microsoft.AspNetCore.Http;

namespace BusesControl.Business.v1;

public class BusBusiness(
    INotificationContext _notificationContext,
    IBusRepository _busRepository 
) : IBusBusiness
{
    public async Task<bool> ExistsByRenavamOrLicensePlateOrChassisAsync(string renavam, string licensePlate, string chassi, Guid? id = null)
    {
        var exists = await _busRepository.ExistsByRenavamOrLicensePlateOrChassisAsync(renavam, licensePlate, chassi, id);
        if (exists)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status409Conflict,
                title: NotificationTitle.Conflict,
                details: Message.Bus.Exists
            );
            return false;
        }

        return true;
    }

    public async Task<bool> ValidateForContractBusReplacementAsync(Guid id)
    {
        var record = await _busRepository.GetByIdAsync(id);
        if (record is null)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Bus.NotFound
            );
            return false;
        }

        if (record.Status != BusStatusEnum.Active)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Bus.NotActive
            );
            return false;
        }

        if (record.Availability != AvailabilityEnum.Available)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Bus.NotAvailable
            );
            return false;
        }

        return true;
    }
}
