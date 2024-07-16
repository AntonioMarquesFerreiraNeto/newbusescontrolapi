using BusesControl.Entities.Models;

namespace BusesControl.Business.v1.Interfaces;

public interface ITerminationBusiness
{
    Task<ContractModel> GetForCreateAsync(Guid contractId, Guid customerId);
}
