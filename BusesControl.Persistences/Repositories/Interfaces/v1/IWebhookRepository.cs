using BusesControl.Entities.Enums.v1;
using BusesControl.Entities.Models.v1;

namespace BusesControl.Persistence.Repositories.Interfaces.v1;

public interface IWebhookRepository : IGenericRepository<WebhookModel>
{
    Task<IEnumerable<WebhookModel>> GetAllAsync();
    Task<WebhookModel?> GetByIdAsync(Guid id);
    Task<WebhookModel?> GetByTypeAsync(WebhookTypeEnum type);
    Task<bool> ExistsByNameOrUrlOrTypeAsync(string name, string url, WebhookTypeEnum type);
}
