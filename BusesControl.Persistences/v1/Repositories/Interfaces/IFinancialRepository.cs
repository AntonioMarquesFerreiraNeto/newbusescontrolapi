using BusesControl.Entities.Models;

namespace BusesControl.Persistence.v1.Repositories.Interfaces;

public interface IFinancialRepository
{
    Task<IEnumerable<FinancialModel>> GetAllAsync();
    Task<IEnumerable<FinancialModel>> FindBySearchAsync(int page = 0, int pageSize = 0, string? search = null);
    Task<FinancialModel?> GetByIdAsync(Guid id);
    Task<FinancialModel?> GetByIdWithInvoicesAsync(Guid id);
    Task<FinancialModel?> GetByIdWithInvoicesExpenseAsync(Guid id);
    Task<FinancialModel?> GetByContractAndCustomerWithInvoicesAsync(Guid contractId, Guid customerId);
    Task<FinancialModel?> GetByIdWithIncludesAsync(Guid id);
    Task<bool> CreateAsync(FinancialModel record);
    Task<bool> CreateRangeAsync(IEnumerable<FinancialModel> records);
    bool Update(FinancialModel record);
    Task<bool> ExistsByReferenceAsync(string reference);
    Task<bool> ExistsBySettingPanelAsync(Guid settingPanelId);
}
