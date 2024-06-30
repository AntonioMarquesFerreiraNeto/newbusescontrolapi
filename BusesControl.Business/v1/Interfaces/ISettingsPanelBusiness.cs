using BusesControl.Entities.Enums;
using BusesControl.Entities.Models;

namespace BusesControl.Business.v1.Interfaces;

public interface ISettingsPanelBusiness
{
    Task<bool> ExistsByParentAsync(SettingsPanelParentEnum parent);
    Task<SettingsPanelModel> GetForUpdateAsync(Guid id, SettingsPanelParentEnum parent);
    Task<SettingsPanelModel> GetForCreateOrUpdateContractAsync(Guid settingsPanelId);
    Task<SettingsPanelModel> GetForDeleteAsync(Guid id);
}
