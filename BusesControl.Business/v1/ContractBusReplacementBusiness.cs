using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Models;
using BusesControl.Entities.Requests;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BusesControl.Business.v1;

public class ContractBusReplacementBusiness(
    INotificationApi _notificationApi,
    IBusBusiness _busBusiness,
    IContractBusiness _contractBusiness
) : IContractBusReplacementBusiness
{
    private bool ValidateStartDateAndTerminateDate(DateOnly startDate, DateOnly terminateDate, ContractModel contract)
    {
        if (terminateDate < startDate)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.ContractBusReplacement.StartDateLessTerminateDate
            );
            return false;
        }

        if (startDate < contract.StartDate)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.ContractBusReplacement.StartDateLessContractStartDate
            );
            return false;
        }

        if (terminateDate > contract.TerminateDate)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.ContractBusReplacement.TerminateDateGreaterContractTerminateDate
            );
            return false;
        }

        return true;
    }

    public async Task<bool> ValidateForCreateAsync(Guid contractId, ContractBusReplacementCreateRequest request)
    {
        var contractRecord = await _contractBusiness.GetForContractBusOrDriverReplacementAsync(contractId);
        if (_notificationApi.HasNotification)
        {
            return false;
        }

        ValidateStartDateAndTerminateDate(request.StartDate, request.TerminateDate, contractRecord);
        if (_notificationApi.HasNotification)
        {
            return false;
        }

        if (request.BusId == contractRecord.BusId)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.ContractBusReplacement.BusInvalid
            );
            return false;
        }

        await _busBusiness.ValidateForContractBusReplacementAsync(request.BusId);
        if (_notificationApi.HasNotification)
        {
            return false;
        }

        return true;
    }
}
