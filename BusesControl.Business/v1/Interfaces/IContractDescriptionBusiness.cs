using BusesControl.Entities.Models;

namespace BusesControl.Business.v1.Interfaces;

public interface IContractDescriptionBusiness
{
    Task<ContractDescriptionModel> GetForUpdateAsync(Guid id);
    Task<ContractDescriptionModel> GetForDeleteAsync(Guid id);
}
