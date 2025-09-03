using BusesControl.Entities.Models.v1;

namespace BusesControl.Persistence.Repositories.Interfaces.v1;

public interface IFinancialRepository : IGenericRepository<FinancialModel>
{
    Task<IEnumerable<FinancialModel>> GetAllAsync();
    Task<IEnumerable<FinancialModel>> FindBySearchAsync(int page = 0, int pageSize = 0, string? search = null);
    Task<IEnumerable<FinancialModel>> FindRecentsByQuantities(int quantities);
    Task<int> CountBySearchAsync(string? search = null);
    Task<FinancialModel?> GetByIdAsync(Guid id);
    Task<FinancialModel?> GetByIdWithInvoicesAsync(Guid id);
    Task<FinancialModel?> GetByIdWithInvoicesExpenseAsync(Guid id);
    Task<FinancialModel?> GetByContractAndCustomerWithInvoicesAsync(Guid contractId, Guid customerId);
    Task<FinancialModel?> GetByIdWithIncludesAsync(Guid id);
    Task<bool> ExistsByReferenceAsync(string reference);
    Task<bool> ExistsBySettingPanelAsync(Guid settingPanelId);
}
