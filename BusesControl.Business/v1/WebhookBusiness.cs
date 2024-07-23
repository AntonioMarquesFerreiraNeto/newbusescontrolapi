using Azure.Core;
using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Enums;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BusesControl.Business.v1;

public class WebhookBusiness(
    INotificationApi _notificationApi,
    IWebhookRepository _webhookRepository
) : IWebhookBusiness
{
    public async Task<bool> ValidateTokenAsync(string? authToken = null, WebhookTypeEnum type = WebhookTypeEnum.Received)
    {
        var record = await _webhookRepository.GetByTypeAsync(type);
        if (record is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Webhook.NotFound
            );
            return false;
        }

        if (authToken is null || !record.AuthToken.Equals(authToken))
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status401Unauthorized,
                title: NotificationTitle.Unauthorized,
                details: Message.Webhook.Unauthorized
            );
            return false;
        }

        return true;
    }
}
