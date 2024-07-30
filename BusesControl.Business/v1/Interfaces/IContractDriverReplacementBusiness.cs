using BusesControl.Entities.Requests;

namespace BusesControl.Business.v1.Interfaces;

public interface IContractDriverReplacementBusiness
{
    Task<bool> ValidateForCreateAsync(Guid contractId, ContractDriverReplacementCreateRequest request);
}
