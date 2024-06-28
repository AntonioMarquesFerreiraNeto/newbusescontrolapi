using BusesControl.Entities.Models;

namespace BusesControl.Business.v1.Interfaces;

public interface IContractBusiness
{
    Task<bool> ValidateTerminationDateAsync(DateTime terminateDate);
    Task<bool> ValidateBusAndEmployeeVinculationAsync(Guid busId, Guid driverId);
    Task<ContractModel> GetForUpdateAsync(Guid id);
    Task<ContractModel> GetForDeleteAsync(Guid id);
    Task<ContractModel> GetForDeniedAsync(Guid id);
    Task<ContractModel> GetForWaitingReviewAsync(Guid id);
    Task<ContractModel> GetForApproveAsync(Guid id);
    bool ValidateDuplicateCustomersValidate(IEnumerable<Guid> customersId);
}
