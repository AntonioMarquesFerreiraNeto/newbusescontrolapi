using BusesControl.Entities.Models;

namespace BusesControl.Persistence.v1.Repositories.Interfaces;

public interface IContractDriverReplacementRepository
{
    Task<IEnumerable<ContractDriverReplacementModel>> FindByContractAsync(Guid contractId);
    Task<ContractDriverReplacementModel?> GetByIdAsync(Guid id);
    Task<ContractDriverReplacementModel?> GetByIdAndContractAsync(Guid id, Guid contractId);
    Task<ContractDriverReplacementModel?> GetByIdAndContractWithDriverAsync(Guid id, Guid contractId);
    Task<bool> CreateAsync(ContractDriverReplacementModel record);
    bool Update(ContractDriverReplacementModel record);
    bool Delete(ContractDriverReplacementModel record);
}
