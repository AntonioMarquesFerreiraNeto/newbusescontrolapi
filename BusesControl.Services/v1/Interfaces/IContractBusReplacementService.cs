using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Requests.v1;
using BusesControl.Entities.Responses.v1;

namespace BusesControl.Services.v1.Interfaces;

public interface IContractBusReplacementService
{
    Task<PaginationResponse<ContractBusReplacementModel>> FindByContractAsync(Guid contractId);
    Task<ContractBusReplacementModel> GetByIdAsync(Guid id, Guid contractId);
    Task<bool> CreateAsync(Guid contractId, ContractBusReplacementCreateRequest request);
    Task<bool> DeleteAsync(Guid contractId, Guid id);
}
