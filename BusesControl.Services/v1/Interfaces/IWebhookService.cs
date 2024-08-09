using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Requests.v1;
using BusesControl.Entities.Responses.v1;

namespace BusesControl.Services.v1.Interfaces;

public interface IWebhookService
{
    Task<PaginationResponse<WebhookResponse>> GetAllAsync();
    Task<WebhookResponse> GetByIdAsync(Guid id);
    Task<bool> CreateAsync(WebhookCreateRequest request);
    Task<WebhookChangeTokenResponse> ChangeInternalAsync(WebhookModel record);
    Task<SuccessResponse> DeleteAsync(Guid id);
    Task<bool> PaymentPixAsync(string? accessToken, PaymentPixRequest request);
}
