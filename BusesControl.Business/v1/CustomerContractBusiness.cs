using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using Microsoft.AspNetCore.Http;

namespace BusesControl.Business.v1;

public class CustomerContractBusiness(
    INotificationContext _notificationContext,
    ICustomerRepository _customerRepository
) : ICustomerContractBusiness
{
    public async Task<bool> ValidateForCreateAsync(Guid customerId)
    {
        var customerRecord = await _customerRepository.GetByIdAsync(customerId);
        if (customerRecord is null)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Customer.NotFound
            );
            return false;
        }

        if (!customerRecord.Active)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Customer.NotActive
            );
            return false;
        }

        return true;
    }
}
