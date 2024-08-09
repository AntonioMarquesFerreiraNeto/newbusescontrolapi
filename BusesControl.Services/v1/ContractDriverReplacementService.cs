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

public class ContractDriverReplacementService(
    IUnitOfWork _unitOfWork,
    INotificationContext _notificationContext,
    IContractDriverReplacementBusiness _contractDriverReplacementBusiness,
    IContractDriverReplacementRepository _contractDriverReplacementRepository
) : IContractDriverReplacementService
{
    public async Task<PaginationResponse<ContractDriverReplacementModel>> FindByContractAsync(Guid contractId)
    {
        var records = await _contractDriverReplacementRepository.FindByContractAsync(contractId);

        return new PaginationResponse<ContractDriverReplacementModel> 
        { 
            Response = records,
            TotalSize = records.Count()
        };
    }

    public async Task<ContractDriverReplacementModel> GetByIdAsync(Guid id, Guid contractId)
    {
        var record = await _contractDriverReplacementRepository.GetByIdAndContractWithDriverAsync(id, contractId);
        if (record is null)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.ContractDriverReplacement.NotFound
            );
            return default!;
        }

        return record;
    }

    public async Task<bool> CreateAsync(Guid contractId, ContractDriverReplacementCreateRequest request)
    {
        await _contractDriverReplacementBusiness.ValidateForCreateAsync(contractId, request);
        if (_notificationContext.HasNotification)
        {
            return false;
        }

        var record = new ContractDriverReplacementModel
        { 
            ContractId = contractId,
            DriverId = request.DriverId,
            StartDate = request.StartDate,
            TerminateDate = request.TerminateDate,
            ReasonDescription = request.ReasonDescription
        };
        await _contractDriverReplacementRepository.AddAsync(record);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(Guid contractId, Guid id)
    {
        var record = await _contractDriverReplacementRepository.GetByIdAndContractAsync(id, contractId);
        if (record is null)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.ContractDriverReplacement.NotFound
            );
            return false;
        }

        _contractDriverReplacementRepository.Remove(record);
        await _unitOfWork.CommitAsync();

        return true;
    }
}
