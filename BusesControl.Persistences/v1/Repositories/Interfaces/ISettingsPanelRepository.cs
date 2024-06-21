using BusesControl.Entities.Enums;
using BusesControl.Entities.Models;

namespace BusesControl.Persistence.v1.Repositories.Interfaces;

public interface ISettingsPanelRepository
{
    Task<SettingsPanelModel?> GetByIdAsync(Guid id);
    Task<IEnumerable<SettingsPanelModel>> FindAsync(int page = 0, int pageSize = 0);
    Task<bool> CreateAsync(SettingsPanelModel record);
    bool Update(SettingsPanelModel record);
    bool Delete(SettingsPanelModel record);
    Task<bool> ExistsByParent(SettingsPanelParentEnum parent, Guid? id = null);
}
