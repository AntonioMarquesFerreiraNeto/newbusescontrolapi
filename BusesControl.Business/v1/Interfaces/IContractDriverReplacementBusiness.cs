using BusesControl.Entities.Requests.v1;

namespace BusesControl.Business.v1.Interfaces;

public interface IContractDriverReplacementBusiness
{
    Task<bool> ValidateForCreateAsync(Guid contractId, ContractDriverReplacementCreateRequest request);
}
