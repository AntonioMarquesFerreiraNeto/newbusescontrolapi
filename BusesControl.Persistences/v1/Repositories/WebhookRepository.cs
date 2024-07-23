using BusesControl.Entities.Enums;
using BusesControl.Entities.Models;
using BusesControl.Persistence.Contexts;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Persistence.v1.Repositories;

public class WebhookRepository(
    AppDbContext context
) : IWebhookRepository
{
    private readonly AppDbContext _context = context;

    public async Task<IEnumerable<WebhookModel>> GetAllAsync()
    {
        return await _context.Webhooks.AsNoTracking().ToListAsync();
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

    public async Task<bool> CreateAsync(WebhookModel record)
    {
        await _context.Webhooks.AddAsync(record);
        return true;
    }

    public bool Update(WebhookModel record)
    {
        _context.Webhooks.Update(record);
        return true;
    }

    public bool Delete(WebhookModel record)
    {
        _context.Webhooks.Remove(record);
        return true;
    }
}
