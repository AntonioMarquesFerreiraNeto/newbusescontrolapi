using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BusesControl.Business.v1;

public class SupplierBusiness(
    INotificationApi _notificationApi,
    ISupplierRepository _supplierRepository
) : ISupplierBusiness
{
    public async Task<bool> ExistsByEmailOrPhoneNumberOrCnpjAsync(string email, string phoneNumber, string cnpj, Guid? id = null)
    {
        var exists = await _supplierRepository.ExistsByEmailOrPhoneNumberOrCnpjAsync(email, phoneNumber, cnpj, id);
        if (exists)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status409Conflict,
                title: NotificationTitle.Conflict,
                details: Message.Supplier.Exists
            );
            return false;
        }

        return true;
    }
}
