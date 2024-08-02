using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Models.v1;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using Microsoft.AspNetCore.Http;

namespace BusesControl.Business.v1;

public class SupplierBusiness(
    INotificationContext _notificationContext,
    ISupplierRepository _supplierRepository
) : ISupplierBusiness
{
    public async Task<bool> ExistsByEmailOrPhoneNumberOrCnpjAsync(string email, string phoneNumber, string cnpj, Guid? id = null)
    {
        var exists = await _supplierRepository.ExistsByEmailOrPhoneNumberOrCnpjAsync(email, phoneNumber, cnpj, id);
        if (exists)
        {
            _notificationContext.SetNotification(
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
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Supplier.NotFound
            );
            return default!;
        }

        if (!record.Active)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Supplier.NotActive
            );
            return default!;
        }

        return record;
    }
}
