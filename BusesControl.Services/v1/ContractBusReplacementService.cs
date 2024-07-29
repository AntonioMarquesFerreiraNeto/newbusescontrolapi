using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Models;
using BusesControl.Entities.Requests;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using BusesControl.Persistence.v1.UnitOfWork;
using BusesControl.Services.v1.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BusesControl.Services.v1;

public class ContractBusReplacementService(
    IUnitOfWork _unitOfWork,
    INotificationApi _notificationApi,
    IContractBusReplacementBusiness _contractBusReplacementBusiness,
    IContractBusReplacementRepository _contractBusReplacementRepository
) : IContractBusReplacementService
{
    public async Task<IEnumerable<ContractBusReplacementModel>> FindByContractAsync(Guid contractId)
    {
        var records = await _contractBusReplacementRepository.FindByContractAsync(contractId);
        return records;
    }

    public async Task<ContractBusReplacementModel> GetByIdAsync(Guid id, Guid contractId)
    {
        var record = await _contractBusReplacementRepository.GetByIdAndContractWithBusAsync(id, contractId);
        if (record is null)
        {
            _notificationApi.SetNotification(
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
        if (_notificationApi.HasNotification)
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
        await _contractBusReplacementRepository.CreateAsync(record);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(Guid contractId, Guid id)
    {
        var record = await _contractBusReplacementRepository.GetByIdAndContractAsync(id, contractId);
        if (record is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.ContractBusReplacement.NotFound
            );
            return false;
        }

        _contractBusReplacementRepository.Delete(record);
        await _unitOfWork.CommitAsync();

        return true;
    }
}
