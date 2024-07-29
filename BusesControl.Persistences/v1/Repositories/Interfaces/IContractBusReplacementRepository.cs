using BusesControl.Entities.Models;

namespace BusesControl.Persistence.v1.Repositories.Interfaces;

public interface IContractBusReplacementRepository
{
    Task<IEnumerable<ContractBusReplacementModel>> FindByContractAsync(Guid contractId);
    Task<ContractBusReplacementModel?> GetByIdAsync(Guid id);
    Task<ContractBusReplacementModel?> GetByIdAndContractAsync(Guid id, Guid contractId);
    Task<ContractBusReplacementModel?> GetByIdAndContractWithBusAsync(Guid id, Guid contractId);
    Task<bool> CreateAsync(ContractBusReplacementModel record);
    bool Update(ContractBusReplacementModel record);
    bool Delete(ContractBusReplacementModel record);
}
