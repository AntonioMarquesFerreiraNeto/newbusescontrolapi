using BusesControl.Entities.Enums.v1;
using BusesControl.Entities.Models.v1;

namespace BusesControl.Persistence.Repositories.Interfaces.v1;

public interface IContractRepository : IGenericRepository<ContractModel>
{
    Task<ContractModel?> GetByIdAsync(Guid id);
    Task<ContractModel?> GetByIdWithCustomersContractAsync(Guid id);
    Task<ContractModel?> GetByIdWithIncludesAsync(Guid id);
    Task<ContractModel?> GetByIdWithSettingPanelAsync(Guid id);
    Task<IEnumerable<ContractModel>> GetAllAsync();
    Task<IEnumerable<ContractModel>> FindByOptionalStatusAsync(int page = 0, int pageSize = 0, ContractStatusEnum? status = null);
    Task<IEnumerable<ContractModel>> FindByContractAndTerminateDateAsync(ContractStatusEnum status, DateOnly terminateDate);
    Task<int> CountByOptionalStatusAsync(ContractStatusEnum? status = null);
    Task<bool> ExistsInIsApprovedBySettingPanelAsync(Guid settingPanelId);
    Task<bool> ExistsBySettingPanelAsync(Guid settingPanelId);
    Task<bool> ExistsInIsApprovedByContractDescriptionAsync(Guid contractDescriptionId);
    Task<bool> ExistsByContractDescriptionAsync(Guid contractDescriptionId);
    Task<bool> ExistsByReferenceAsync(string reference);
}
