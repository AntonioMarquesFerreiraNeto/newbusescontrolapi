using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Requests.v1;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using Microsoft.AspNetCore.Http;

namespace BusesControl.Business.v1;

public class CustomerBusiness(
    INotificationContext _notificationContext,
    ICustomerRepository _customerRepository
) : ICustomerBusiness
{
    public async Task<bool> ExistsByRequestAsync(CustomerCreateRequest request, Guid? id = null)
    {
        var exists = await _customerRepository.ExistsByEmailOrPhoneNumberOrCpfOrCnpjAsync(request.Email, request.PhoneNumber, request.Cpf, request.Cnpj, id);
        if (exists)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status409Conflict,
                title: NotificationTitle.Conflict,
                details: Message.Customer.Exists
            );
            return false;
        }

        return true;
    }

    public async Task<CustomerModel> GetWithValidateActiveAsync(Guid id)
    {
        var record = await _customerRepository.GetByIdAsync(id);
        if (record is null)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Customer.NotFound
            );
            return default!;
        }

        if (!record.Active)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Customer.NotActive
            );
            return default!;
        }

        return record;
    }
}
