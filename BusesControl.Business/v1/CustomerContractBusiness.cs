using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BusesControl.Business.v1;

public class CustomerContractBusiness(
    INotificationApi _notificationApi,
    ICustomerRepository _customerRepository
) : ICustomerContractBusiness
{
    public async Task<bool> ValidateForCreateAsync(Guid customerId)
    {
        var customerRecord = await _customerRepository.GetByIdAsync(customerId);
        if (customerRecord is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Customer.NotFound
            );
            return false;
        }

        if (customerRecord.InCompliance)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Customer.NotInCompliance
            );
            return false;
        }

        return true;
    }
}
