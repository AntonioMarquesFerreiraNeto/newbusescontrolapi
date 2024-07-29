using BusesControl.Entities.Requests;

namespace BusesControl.Business.v1.Interfaces;

public interface IContractBusReplacementBusiness
{
    Task<bool> ValidateForCreateAsync(Guid contractId, ContractBusReplacementCreateRequest request);
}
