using BusesControl.Entities.Models.v1;

namespace BusesControl.Persistence.Repositories.Interfaces.v1;

public interface IContractDriverReplacementRepository : IGenericRepository<ContractDriverReplacementModel>
{
    Task<IEnumerable<ContractDriverReplacementModel>> FindByContractAsync(Guid contractId);
    Task<ContractDriverReplacementModel?> GetByIdAsync(Guid id);
    Task<ContractDriverReplacementModel?> GetByIdAndContractAsync(Guid id, Guid contractId);
    Task<ContractDriverReplacementModel?> GetByIdAndContractWithDriverAsync(Guid id, Guid contractId);
}
