using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Requests.v1;
using BusesControl.Entities.Responses.v1;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using BusesControl.Persistence.UnitOfWork;
using BusesControl.Services.v1.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BusesControl.Services.v1;

public class ContractBusReplacementService(
    IUnitOfWork _unitOfWork,
    INotificationContext _notificationContext,
    IContractBusReplacementBusiness _contractBusReplacementBusiness,
    IContractBusReplacementRepository _contractBusReplacementRepository
) : IContractBusReplacementService
{
    public async Task<PaginationResponse<ContractBusReplacementModel>> FindByContractAsync(Guid contractId)
    {
        var records = await _contractBusReplacementRepository.FindByContractAsync(contractId);
        return new PaginationResponse<ContractBusReplacementModel> 
        { 
            Response = records,
            TotalSize = records.Count()
        };
    }

    public async Task<ContractBusReplacementModel> GetByIdAsync(Guid id, Guid contractId)
    {
        var record = await _contractBusReplacementRepository.GetByIdAndContractWithBusAsync(id, contractId);
        if (record is null)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.ContractBusReplacement.NotFound
            );
            return default!;
        }

        return record;
    }

    public async Task<bool> CreateAsync(Guid contractId, ContractBusReplacementCreateRequest request)
    {
        await _contractBusReplacementBusiness.ValidateForCreateAsync(contractId, request);
        if (_notificationContext.HasNotification)
        {
            return false;
        }

        var record = new ContractBusReplacementModel
        {
            ContractId = contractId,
            BusId = request.BusId,
            StartDate = request.StartDate,
            TerminateDate = request.TerminateDate,
            ReasonDescription = request.ReasonDescription
        };
        await _contractBusReplacementRepository.AddAsync(record);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(Guid contractId, Guid id)
    {
        var record = await _contractBusReplacementRepository.GetByIdAndContractAsync(id, contractId);
        if (record is null)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.ContractBusReplacement.NotFound
            );
            return false;
        }

        _contractBusReplacementRepository.Remove(record);
        await _unitOfWork.CommitAsync();

        return true;
    }
}
