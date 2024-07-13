using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Enums;
using BusesControl.Entities.Models;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BusesControl.Business.v1;

public class InvoiceBusiness(
    INotificationApi _notificationApi,
    IInvoiceRepository _invoiceRepository
) : IInvoiceBusiness
{
    public async Task<InvoiceModel> GetForPaymentAsync(Guid id)
    {
        var record = await _invoiceRepository.GetByIdWithFinancialAsync(id);
        if (record is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Invoice.NotFound
            );
            return default!;
        }

        if (record.Status != InvoiceStatusEnum.Pending && record.Status != InvoiceStatusEnum.OverDue)
        {
            _notificationApi.SetNotification(
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
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Invoice.NotFound
            );
            return default!;
        }

        if (record.Status != InvoiceStatusEnum.Pending && record.Status != InvoiceStatusEnum.OverDue)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Invoice.NotPendingOrOverdue
            );
            return default!;
        }

        return record;
    }
}
