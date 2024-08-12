using BusesControl.Entities.Enums.v1;
using BusesControl.Entities.Models.v1;
using BusesControl.Persistence.Contexts;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Persistence.Repositories.v1;

public class WebhookRepository(
    AppDbContext context
) : GenericRepository<WebhookModel>(context), IWebhookRepository
{
    private readonly AppDbContext _context = context;

    public async Task<IEnumerable<WebhookModel>> GetAllAsync(string? search = null)
    {
        var query = _context.Webhooks.AsNoTracking();

        if (search is not null)
        {
            query = query.Where(x => x.Name.Contains(search));
        }

        return await query.ToListAsync();
    }

    public async Task<WebhookModel?> GetByIdAsync(Guid id)
    {
        return await _context.Webhooks.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<WebhookModel?> GetByTypeAsync(WebhookTypeEnum type)
    {
        return await _context.Webhooks.AsNoTracking().SingleOrDefaultAsync(x => x.Type == type);
    }

    public async Task<bool> ExistsByNameOrUrlOrTypeAsync(string name, string url, WebhookTypeEnum type)
    {
        return await _context.Webhooks.AsNoTracking().AnyAsync(x => x.Name == name || x.Url == url || x.Type == type);
    }
}
