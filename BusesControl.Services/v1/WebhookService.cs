using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Enums;
using BusesControl.Entities.Requests;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using BusesControl.Persistence.v1.UnitOfWork;
using BusesControl.Services.v1.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BusesControl.Services.v1;

public class WebhookService(
    AppSettings _appSettings,
    IUnitOfWork _unitOfWork,
    INotificationApi _notificationApi,
    IInvoiceBusiness _invoiceBusiness,
    IInvoiceRepository _invoiceRepository
) : IWebhookService
{
    public async Task<bool> PaymentPixAsync(string? accessToken, PaymentPixRequest request)
    {
        if (accessToken is null || accessToken != _appSettings.WebhookAssas.AccessToken)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status401Unauthorized,
                title: NotificationTitle.Unauthorized,
                details: Message.Webhook.Unauthorized
            );
            return false;
        }

        if (request.Payment is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Webhook.RequestRequired
            );
            return false;
        }

        if (request.Event != "PAYMENT_RECEIVED")
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Webhook.EventNotAccepted
            );
            return false;
        }

        var record = await _invoiceBusiness.GetForPaymentPixAsync(request.Payment.ExternalReference, request.Payment.Id);
        if (_notificationApi.HasNotification)
        {
            return false;
        }

        record.UpdatedAt = DateTime.UtcNow;
        record.PaymentDate = request.Payment.PaymentDate ?? DateOnly.FromDateTime(DateTime.UtcNow);
        record.Status = InvoiceStatusEnum.Paid;
        _invoiceRepository.Update(record);
        await _unitOfWork.CommitAsync();

        return true;
    }
}
