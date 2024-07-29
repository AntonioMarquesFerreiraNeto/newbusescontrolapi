using BusesControl.Entities.Models;

namespace BusesControl.Business.v1.Interfaces;

public interface IContractBusiness
{
    Task<bool> ValidateBusAndEmployeeVinculationAsync(Guid busId, Guid driverId);
    Task<CustomerContractModel> GetForGeneratedContractForCustomerAsync(Guid id, Guid customerId);
    Task<CustomerContractModel> GetForGeneratedTerminationForCustomerAsync(Guid id, Guid customerId);
    Task<ContractModel> GetForUpdateAsync(Guid id);
    Task<ContractModel> GetForDeleteAsync(Guid id);
    Task<ContractModel> GetForDeniedAsync(Guid id);
    Task<ContractModel> GetForWaitingReviewAsync(Guid id);
    Task<ContractModel> GetForApproveAsync(Guid id);
    Task<ContractModel> GetForStartProgressAsync(Guid id);
    Task<ContractModel> GetForContractBusOrDriverReplacementAsync(Guid contractId);
    bool ValidateTerminationDate(SettingPanelModel settingPanelRecord, DateOnly terminateDate);
    bool ValidateDuplicateCustomersValidate(IEnumerable<Guid> customersId);
}
