using BusesControl.Entities.Enums;
using BusesControl.Entities.Models;

namespace BusesControl.Persistence.v1.Repositories.Interfaces;

public interface IContractRepository
{
    Task<ContractModel?> GetByIdAsync(Guid id);
    Task<ContractModel?> GetByIdWithIncludesAsync(Guid id);
    Task<IEnumerable<ContractModel>> FindByOptionalStatusAsync(int page = 0, int pageSize = 0, ContractStatusEnum? status = null);
    Task<bool> CreateAsync(ContractModel record);
    bool Update(ContractModel record);
    bool Delete(ContractModel record);
    Task<bool> ExistsInIsApprovedBySettingsPanelAsync(Guid settingsPanelId);
    Task<bool> ExistsBySettingsPanelAsync(Guid settingsPanelId);
    Task<bool> ExitsByReferenceAsync(string reference);
}
