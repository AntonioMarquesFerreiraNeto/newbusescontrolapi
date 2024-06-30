using BusesControl.Entities.Models;

namespace BusesControl.Services.v1.Interfaces;

public interface ICustomerContractService
{
    Task<bool> CreateForContractAsync(IEnumerable<Guid> customersId, Guid contractId);
    Task<bool> UpdateForContractAsync(IEnumerable<Guid> customersId, Guid contractId);
    Task<bool> StartProcessTerminationWithOutValidationAsync(CustomerContractModel record);
}
