using BusesControl.Entities.Enums.v1;
using BusesControl.Entities.Models.v1;
using BusesControl.Persistence.Contexts;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Persistence.Repositories.v1;

public class SettingPanelRepository(
    AppDbContext context
) : GenericRepository<SettingPanelModel>(context), ISettingPanelRepository
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

    public async Task<bool> ExistsByParentExceptionContract(SettingPanelParentEnum parent, Guid? id = null)
    {
        return await _context.SettingsPanel.AnyAsync(x => x.Parent == parent && x.Id != id && x.Parent != SettingPanelParentEnum.Contract);
    }

    public async Task<bool> ExitsByReferenceAsync(string reference)
    {
        return await _context.SettingsPanel.AnyAsync(x => x.Reference == reference);
    }
}
