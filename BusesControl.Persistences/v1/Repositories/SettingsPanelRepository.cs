using BusesControl.Entities.Enums;
using BusesControl.Entities.Models;
using BusesControl.Persistence.Contexts;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Persistence.v1.Repositories;

public class SettingsPanelRepository(
    AppDbContext context
) : ISettingsPanelRepository
{
    private readonly AppDbContext _context = context;

    public async Task<SettingsPanelModel?> GetByIdAsync(Guid id)
    {
        return await _context.SettingsPanel.AsNoTracking().Include(x => x.Requester).SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<SettingsPanelModel?> GetByParentAsync(SettingsPanelParentEnum parent)
    {
        return await _context.SettingsPanel.AsNoTracking().SingleOrDefaultAsync(x => x.Parent == parent);
    }

    public async Task<IEnumerable<SettingsPanelModel>> FindAsync(int page = 0, int pageSize = 0)
    {
        var query = _context.SettingsPanel.AsNoTracking();

        if (page > 0 & pageSize > 0)
        {
            query = query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        var records = await query.ToListAsync();

        return records;
    }

    public async Task<bool> CreateAsync(SettingsPanelModel record)
    {
        await _context.SettingsPanel.AddAsync(record);
        return true;
    }

    public bool Update(SettingsPanelModel record)
    {
        _context.SettingsPanel.Update(record);
        return true;
    }

    public bool Delete(SettingsPanelModel record)
    {
        _context.SettingsPanel.Remove(record);
        return true;
    }

    public async Task<bool> ExistsByParent(SettingsPanelParentEnum parent, Guid? id = null)
    {
        return await _context.SettingsPanel.AnyAsync(x => x.Parent == parent && x.Id != id);
    }
}
