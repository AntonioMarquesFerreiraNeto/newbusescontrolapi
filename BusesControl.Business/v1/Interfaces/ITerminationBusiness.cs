using BusesControl.Entities.Models;

namespace BusesControl.Business.v1.Interfaces;

public interface ITerminationBusiness
{
    Task<ContractModel> GetContractForCreateAsync(Guid contractId);
    Task<CustomerContractModel> GetCustomerContractForCreateAsync(Guid contractId, Guid customerId);
}
