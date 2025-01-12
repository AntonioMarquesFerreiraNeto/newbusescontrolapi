using BusesControl.Entities.Enums.v1;
using BusesControl.Entities.Models.v1;

namespace BusesControl.Persistence.Repositories.Interfaces.v1;

public interface ISettingPanelRepository : IGenericRepository<SettingPanelModel>
{
    Task<SettingPanelModel?> GetByIdAsync(Guid id);
    Task<IEnumerable<SettingPanelModel>> FindAsync(int page = 0, int pageSize = 0);
    Task<IEnumerable<SettingPanelModel>> FindByParentAysnc(SettingPanelParentEnum parent);
    Task<int> CountAsync();
    Task<bool> ExistsByParentExceptionContract(SettingPanelParentEnum parent, Guid? id = null);
    Task<bool> ExitsByReferenceAsync(string reference);
}
