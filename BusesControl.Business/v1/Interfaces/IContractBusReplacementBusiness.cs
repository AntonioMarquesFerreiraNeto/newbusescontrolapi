using BusesControl.Entities.Requests.v1;

namespace BusesControl.Business.v1.Interfaces;

public interface IContractBusReplacementBusiness
{
    Task<bool> ValidateForCreateAsync(Guid contractId, ContractBusReplacementCreateRequest request);
}
