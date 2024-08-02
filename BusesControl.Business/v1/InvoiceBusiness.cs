using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Enums.v1;
using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Responses.v1;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using Microsoft.AspNetCore.Http;

namespace BusesControl.Business.v1;

public class InvoiceBusiness(
    INotificationContext _notificationContext,
    IInvoiceRepository _invoiceRepository
) : IInvoiceBusiness
{
    public async Task<InvoiceModel> GetForPaymentAsync(Guid id)
    {
        var record = await _invoiceRepository.GetByIdWithFinancialAsync(id);
        if (record is null)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Invoice.NotFound
            );
            return default!;
        }

        if (record.Status != InvoiceStatusEnum.Pending && record.Status != InvoiceStatusEnum.OverDue)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Invoice.NotPendingOrOverdue
            );
            return default!;
        }

        return record;
    }

    public async Task<InvoiceModel> GetForPaymentPixAsync(Guid id, string externalId)
    {
        var record = await _invoiceRepository.GetByIdAndExternalAsync(id, externalId);
        if (record is null)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Invoice.NotFound
            );
            return default!;
        }

        if (record.Status != InvoiceStatusEnum.Pending && record.Status != InvoiceStatusEnum.OverDue)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Invoice.NotPendingOrOverdue
            );
            return default!;
        }

        return record;
    }

    public bool ValidateLoggedUserForJustCountPayment(UserAuthResponse loggedUser)
    {
        if (loggedUser.Role != "Admin" && loggedUser.Role != "Assistant")
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Invoice.JustCountInternalUsersOnly
            );
            return false;
        }

        return true;
    }
}
