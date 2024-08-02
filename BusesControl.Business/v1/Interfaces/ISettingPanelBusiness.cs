using BusesControl.Entities.Enums.v1;
using BusesControl.Entities.Models.v1;

namespace BusesControl.Business.v1.Interfaces;

public interface ISettingPanelBusiness
{
    Task<SettingPanelModel> GetForUpdateAsync(Guid id, SettingPanelParentEnum parent);
    Task<SettingPanelModel> GetForCreateOrUpdateContractAsync(Guid settingPanelId);
    Task<SettingPanelModel> GetForCreateFinancialRevenueAsync(Guid settingPanelId);
    Task<SettingPanelModel> GetForCreateFinancialExpenseAsync(Guid settingPanelId);
    Task<SettingPanelModel> GetForDeleteAsync(Guid id);
}
