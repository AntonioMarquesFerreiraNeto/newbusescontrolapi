using BusesControl.Entities.Enums;
using BusesControl.Entities.Models;

namespace BusesControl.Business.v1.Interfaces;

public interface ISettingPanelBusiness
{
    Task<SettingPanelModel> GetForUpdateAsync(Guid id, SettingPanelParentEnum parent);
    Task<SettingPanelModel> GetForCreateOrUpdateContractAsync(Guid settingPanelId);
    Task<SettingPanelModel> GetForCreateFinancialRevenueAsync(Guid settingPanelId);
    Task<SettingPanelModel> GetForCreateFinancialExpenseAsync(Guid settingPanelId);
    Task<SettingPanelModel> GetForDeleteAsync(Guid id);
}
