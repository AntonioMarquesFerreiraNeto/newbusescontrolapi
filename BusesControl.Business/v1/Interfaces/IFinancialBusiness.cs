using BusesControl.Entities.Models;

namespace BusesControl.Business.v1.Interfaces;

public interface IFinancialBusiness
{
    bool ValidateTerminationDate(SettingPanelModel settingPanelRecord, DateOnly terminateDate);
    Task<FinancialModel> GetForInactiveRevenueAsync(Guid id);
    Task<FinancialModel> GetForUpdateDetailsAsync(Guid id);
}
