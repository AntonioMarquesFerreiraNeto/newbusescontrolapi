using BusesControl.Entities.Models;

namespace BusesControl.Business.v1.Interfaces;

public interface IContractBusiness
{
    Task<bool> ValidateForCreateAsync(Guid busId, Guid driverId);
    Task<ContractModel> GetForApproveAsync(Guid id);
}
