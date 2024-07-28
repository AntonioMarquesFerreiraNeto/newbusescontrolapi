using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Models;
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

    public async Task<SupplierModel> GetWithValidateActiveAsync(Guid id)
    {
        var record = await _supplierRepository.GetByIdAsync(id);
        if (record is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Supplier.NotFound
            );
            return default!;
        }

        if (!record.Active)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Supplier.NotActive
            );
            return default!;
        }

        return record;
    }
}
