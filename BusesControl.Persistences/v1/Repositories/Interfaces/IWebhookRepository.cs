using BusesControl.Entities.Enums;
using BusesControl.Entities.Models;

namespace BusesControl.Persistence.v1.Repositories.Interfaces;

public interface IWebhookRepository
{
    Task<IEnumerable<WebhookModel>> GetAllAsync();
    Task<WebhookModel?> GetByIdAsync(Guid id);
    Task<WebhookModel?> GetByTypeAsync(WebhookTypeEnum type);
    Task<bool> ExistsByNameOrUrlOrTypeAsync(string name, string url, WebhookTypeEnum type);
    Task<bool> CreateAsync(WebhookModel record);
    bool Update(WebhookModel record);
    bool Delete(WebhookModel record);
}
