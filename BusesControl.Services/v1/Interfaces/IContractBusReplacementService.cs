using BusesControl.Entities.Models;
using BusesControl.Entities.Requests;

namespace BusesControl.Services.v1.Interfaces;

public interface IContractBusReplacementService
{
    Task<IEnumerable<ContractBusReplacementModel>> FindByContractAsync(Guid contractId);
    Task<ContractBusReplacementModel> GetByIdAsync(Guid id, Guid contractId);
    Task<bool> CreateAsync(Guid contractId, ContractBusReplacementCreateRequest request);
    Task<bool> DeleteAsync(Guid contractId, Guid id);
}
