using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Models;
using BusesControl.Entities.Requests;
using BusesControl.Entities.Response;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using BusesControl.Persistence.v1.UnitOfWork;
using BusesControl.Services.v1.Interfaces;

namespace BusesControl.Services.v1;

public class TerminationService(
    IUnitOfWork _unitOfWork,
    INotificationApi _notificationApi,
    ICustomerContractService _customerContractService,
    IFinancialService _financialService,
    ITerminationBusiness _terminationBusiness,
    ITerminationRepository _terminationRepository
) : ITerminationService
{
    public async Task<IEnumerable<TerminationModel>> FindByContractAsync(Guid contractId, string? search)
    {
        var records = await _terminationRepository.FindByContractAsync(contractId, search);
        return records;
    }

    public async Task<SuccessResponse> CreateAsync(Guid contractId, TerminationCreateRequest request)
    {
        var contractRecord = await _terminationBusiness.GetContractForCreateAsync(contractId);
        if (_notificationApi.HasNotification)
        {
            return default!;
        }

        var customerContractRecord = await _terminationBusiness.GetCustomerContractForCreateAsync(contractId, request.CustomerId);
        if (_notificationApi.HasNotification)
        {
            return default!;
        }

        _unitOfWork.BeginTransaction();

        await _financialService.ToggleActiveForTerminationAsync(contractId, request.CustomerId);
        if (_notificationApi.HasNotification)
        {
            return default!;
        }

        await _customerContractService.ToggleActiveForTerminationAsync(customerContractRecord);

        var record = new TerminationModel 
        { 
            ContractId = contractId,
            CustomerId = request.CustomerId,
            Price = Math.Round(contractRecord.TotalPrice * contractRecord.SettingPanel.TerminationFee / 100, 2)
        };
        await _terminationRepository.CreateAsync(record);
        await _unitOfWork.CommitAsync();

        await _unitOfWork.CommitAsync(true);

        return new SuccessResponse(Message.Termination.Success);
    }
}
