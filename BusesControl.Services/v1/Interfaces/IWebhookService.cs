using BusesControl.Entities.Requests;

namespace BusesControl.Services.v1.Interfaces;

public interface IWebhookService
{
    Task<bool> PaymentPixAsync(string? accessToken, PaymentPixRequest request);
}
