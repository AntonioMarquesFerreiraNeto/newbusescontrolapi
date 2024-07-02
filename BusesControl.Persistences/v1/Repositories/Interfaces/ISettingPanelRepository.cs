using BusesControl.Entities.Enums;
using BusesControl.Entities.Models;

namespace BusesControl.Persistence.v1.Repositories.Interfaces;

public interface ISettingPanelRepository
{
    Task<SettingPanelModel?> GetByIdAsync(Guid id);
    Task<IEnumerable<SettingPanelModel>> FindAsync(int page = 0, int pageSize = 0);
    Task<bool> CreateAsync(SettingPanelModel record);
    bool Update(SettingPanelModel record);
    bool Delete(SettingPanelModel record);
    Task<bool> ExistsByParentExceptionContract(SettingPanelParentEnum parent, Guid? id = null);
    Task<bool> ExitsByReferenceAsync(string reference);
}
