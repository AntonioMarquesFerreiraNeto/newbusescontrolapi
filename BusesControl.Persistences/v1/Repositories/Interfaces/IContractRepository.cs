using BusesControl.Entities.Enums;
using BusesControl.Entities.Models;

namespace BusesControl.Persistence.v1.Repositories.Interfaces;

public interface IContractRepository
{
    Task<ContractModel?> GetByIdAsync(Guid id);
    Task<ContractModel?> GetByIdWithCustomersContractAsync(Guid id);
    Task<ContractModel?> GetByIdWithIncludesAsync(Guid id);
    Task<ContractModel?> GetByIdWithSettingPanelAsync(Guid id);
    Task<IEnumerable<ContractModel>> FindByOptionalStatusAsync(int page = 0, int pageSize = 0, ContractStatusEnum? status = null);
    Task<IEnumerable<ContractModel>> FindByContractAndTerminateDateAsync(ContractStatusEnum status, DateOnly terminateDate);
    Task<bool> CreateAsync(ContractModel record);
    bool Update(ContractModel record);
    bool Delete(ContractModel record);
    Task<bool> ExistsInIsApprovedBySettingPanelAsync(Guid settingPanelId);
    Task<bool> ExistsBySettingPanelAsync(Guid settingPanelId);
    Task<bool> ExistsInIsApprovedByContractDescriptionAsync(Guid contractDescriptionId);
    Task<bool> ExistsByContractDescriptionAsync(Guid contractDescriptionId);
    Task<bool> ExistsByReferenceAsync(string reference);
}
