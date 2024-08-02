using BusesControl.Entities.Models.v1;

namespace BusesControl.Persistence.Repositories.Interfaces.v1;

public interface IContractBusReplacementRepository : IGenericRepository<ContractBusReplacementModel>
{
    Task<IEnumerable<ContractBusReplacementModel>> FindByContractAsync(Guid contractId);
    Task<ContractBusReplacementModel?> GetByIdAsync(Guid id);
    Task<ContractBusReplacementModel?> GetByIdAndContractAsync(Guid id, Guid contractId);
    Task<ContractBusReplacementModel?> GetByIdAndContractWithBusAsync(Guid id, Guid contractId);
}
