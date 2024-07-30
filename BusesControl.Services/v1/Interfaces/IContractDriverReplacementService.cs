using BusesControl.Entities.Models;
using BusesControl.Entities.Requests;

namespace BusesControl.Services.v1.Interfaces;

public interface IContractDriverReplacementService
{
    Task<IEnumerable<ContractDriverReplacementModel>> FindByContractAsync(Guid contractId);
    Task<ContractDriverReplacementModel> GetByIdAsync(Guid id, Guid contractId);
    Task<bool> CreateAsync(Guid contractId, ContractDriverReplacementCreateRequest request);
    Task<bool> DeleteAsync(Guid contractId, Guid id);
}
