using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BusesControl.Business.v1;

public class CustomerBusiness(
    INotificationApi _notificationApi,
    ICustomerRepository _customerRepository
) : ICustomerBusiness
{
    public async Task<bool> ExistsByEmailOrPhoneNumberOrCpfOrCnpjAsync(string email, string phoneNumber, string? cpf, string? cnpj, Guid? id = null)
    {
        var exists = await _customerRepository.ExistsByEmailOrPhoneNumberOrCpfOrCnpjAsync(email, phoneNumber, cpf, cnpj, id);
        if (exists)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status409Conflict,
                title: NotificationTitle.Conflict,
                details: Message.Customer.Exists
            );
            return false;
        }

        return true;
    }
}
