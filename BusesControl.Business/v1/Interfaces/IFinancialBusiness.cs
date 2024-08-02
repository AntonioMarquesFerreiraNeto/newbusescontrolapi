using BusesControl.Entities.Models.v1;

namespace BusesControl.Business.v1.Interfaces;

public interface IFinancialBusiness
{
    bool ValidateTerminationDate(SettingPanelModel settingPanelRecord, DateOnly terminateDate);
    Task<FinancialModel> GetForInactiveRevenueAsync(Guid id);
    Task<FinancialModel> GetForInactiveExpenseAsync(Guid id);
    Task<FinancialModel> GetForUpdateDetailsAsync(Guid id);
}
