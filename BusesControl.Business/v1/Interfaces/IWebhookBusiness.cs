using BusesControl.Entities.Enums;

namespace BusesControl.Business.v1.Interfaces;

public interface IWebhookBusiness
{
    Task<bool> ValidateTokenAsync(string? authToken = null, WebhookTypeEnum type = WebhookTypeEnum.Received);
}
