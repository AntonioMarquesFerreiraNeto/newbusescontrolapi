using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Models.v1;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using BusesControl.Persistence.UnitOfWork;
using BusesControl.Services.v1.Interfaces;

namespace BusesControl.Services.v1;

public class CustomerContractService(
    IUnitOfWork _unitOfWork,
    INotificationContext _notificationContext,
    ICustomerContractBusiness _customerContractBusiness,
    ICustomerContractRepository _customerContractRepository
) : ICustomerContractService
{
    private async Task<bool> DeleteWithoutValidationAsync(IEnumerable<CustomerContractModel> customersContract)
    {
        _customerContractRepository.RemoveRange(customersContract);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> CreateForContractAsync(IEnumerable<Guid> customersId, Guid contractId)
    {
        var records = new List<CustomerContractModel>();

        foreach (var customerId in customersId)
        {
            await _customerContractBusiness.ValidateForCreateAsync(customerId);
            if (_notificationContext.HasNotification)
            {
                return false;
            }

            var record = new CustomerContractModel
            {
                ContractId = contractId,
                CustomerId = customerId
            };

            records.Add(record);
        }

        await _customerContractRepository.AddRangeAsync(records);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> UpdateForContractAsync(IEnumerable<Guid> customersId, Guid contractId)
    {
        var customersContractRecords = await _customerContractRepository.FindByContractAsync(contractId);

        var newCustomersId = customersId.Where(x => !customersContractRecords.Any(y => y.CustomerId == x));
        var removeCustomersContract = customersContractRecords.Where(x => !customersId.Any(y => y == x.CustomerId));

        await CreateForContractAsync(newCustomersId, contractId);
        if (_notificationContext.HasNotification)
        {
            return false;
        }

        await DeleteWithoutValidationAsync(removeCustomersContract);

        return true;
    }

    public async Task<bool> ToggleProcessTerminationWithOutValidationAsync(CustomerContractModel record)
    {
        record.ProcessTermination = !record.ProcessTermination;
        record.ProcessTerminationDate = record.ProcessTerminationDate is null ? DateTime.UtcNow : null;
        _customerContractRepository.Update(record);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> ToggleActiveForTerminationAsync(CustomerContractModel record)
    {
        record.Active = false;
        _customerContractRepository.Update(record);
        await _unitOfWork.CommitAsync();

        return true;
    }
}
