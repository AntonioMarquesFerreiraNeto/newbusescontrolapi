using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Requests.v1;
using BusesControl.Entities.Responses.v1;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using BusesControl.Persistence.UnitOfWork;
using BusesControl.Services.v1.Interfaces;

namespace BusesControl.Services.v1;

public class TerminationService(
    IUnitOfWork _unitOfWork,
    INotificationContext _notificationContext,
    ICustomerContractService _customerContractService,
    IFinancialService _financialService,
    ITerminationBusiness _terminationBusiness,
    ITerminationRepository _terminationRepository
) : ITerminationService
{
    public async Task<PaginationResponse<TerminationModel>> FindByContractAsync(Guid contractId, string? search)
    {
        var records = await _terminationRepository.FindByContractAsync(contractId, search);

        return new PaginationResponse<TerminationModel> 
        { 
            Response = records,
            TotalSize = records.Count()
        };
    }

    public async Task<SuccessResponse> CreateAsync(Guid contractId, TerminationCreateRequest request)
    {
        var contractRecord = await _terminationBusiness.GetContractForCreateAsync(contractId);
        if (_notificationContext.HasNotification)
        {
            return default!;
        }

        var customerContractRecord = await _terminationBusiness.GetCustomerContractForCreateAsync(contractId, request.CustomerId);
        if (_notificationContext.HasNotification)
        {
            return default!;
        }

        _unitOfWork.BeginTransaction();

        await _financialService.InactiveInternalAsync(contractId, request.CustomerId);
        if (_notificationContext.HasNotification)
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
        await _terminationRepository.AddAsync(record);
        await _unitOfWork.CommitAsync();

        await _unitOfWork.CommitAsync(true);

        return new SuccessResponse(Message.Termination.Success);
    }
}
