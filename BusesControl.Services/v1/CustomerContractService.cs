using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Models;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using BusesControl.Persistence.v1.UnitOfWork;
using BusesControl.Services.v1.Interfaces;

namespace BusesControl.Services.v1;

public class CustomerContractService(
    IUnitOfWork _unitOfWork,
    INotificationApi _notificationApi,
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

        foreach (var customer in customersId)
        {
            await _customerContractBusiness.ValidateForCreateAsync(customer);
            if (_notificationApi.HasNotification)
            {
                return false;
            }

            var record = new CustomerContractModel
            {
                ContractId = contractId,
                CustomerId = customer,
            };
            records.Add(record);
        }

        await _customerContractRepository.CreateRangeAsync(records);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> UpdateForContractAsync(IEnumerable<Guid> customersId, Guid contractId)
    {
        var customersContractRecords = await _customerContractRepository.FindByContractAsync(contractId);

        var newCustomersId = customersId.Where(x => !customersContractRecords.Any(y => y.CustomerId == x));
        var removeCustomersContract = customersContractRecords.Where(x => !customersId.Any(y => y == x.CustomerId));

        await CreateForContractAsync(newCustomersId, contractId);
        if (_notificationApi.HasNotification)
        {
            return false;
        }

        await DeleteWithoutValidationAsync(removeCustomersContract);

        return true;
    }
}
