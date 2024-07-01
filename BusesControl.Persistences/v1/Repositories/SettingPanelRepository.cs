using BusesControl.Entities.Enums;
using BusesControl.Entities.Models;
using BusesControl.Persistence.Contexts;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Persistence.v1.Repositories;

public class SettingPanelRepository(
    AppDbContext context
) : ISettingPanelRepository
{
    private readonly AppDbContext _context = context;

    public async Task<SettingPanelModel?> GetByIdAsync(Guid id)
    {
        return await _context.SettingsPanel.AsNoTracking().Include(x => x.Requester).SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<SettingPanelModel>> FindAsync(int page = 0, int pageSize = 0)
    {
        var query = _context.SettingsPanel.AsNoTracking();

        if (page > 0 & pageSize > 0)
        {
            query = query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        var records = await query.ToListAsync();

        return records;
    }

    public async Task<bool> CreateAsync(SettingPanelModel record)
    {
        await _context.SettingsPanel.AddAsync(record);
        return true;
    }

    public bool Update(SettingPanelModel record)
    {
        _context.SettingsPanel.Update(record);
        return true;
    }

    public bool Delete(SettingPanelModel record)
    {
        _context.SettingsPanel.Remove(record);
        return true;
    }

    public async Task<bool> ExistsByParentExceptionContract(SettingPanelParentEnum parent, Guid? id = null)
    {
        return await _context.SettingsPanel.AnyAsync(x => x.Parent == parent && x.Id != id && x.Parent != SettingPanelParentEnum.Contract);
    }

    public async Task<bool> ExitsByReferenceAsync(string reference)
    {
        return await _context.SettingsPanel.AnyAsync(x => x.Reference == reference);
    }
}
