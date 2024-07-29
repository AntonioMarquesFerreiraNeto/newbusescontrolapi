using BusesControl.Entities.Models;
using BusesControl.Entities.Requests;
using BusesControl.Entities.Response;
using BusesControl.Entities.Responses;

namespace BusesControl.Services.v1.Interfaces;

public interface IWebhookService
{
    Task<IEnumerable<WebhookResponse>> GetAllAsync();
    Task<WebhookResponse> GetByIdAsync(Guid id);
    Task<bool> CreateAsync(WebhookCreateRequest request);
    Task<WebhookChangeTokenResponse> ChangeInternalAsync(WebhookModel record);
    Task<SuccessResponse> DeleteAsync(Guid id);
    Task<bool> PaymentPixAsync(string? accessToken, PaymentPixRequest request);
}
