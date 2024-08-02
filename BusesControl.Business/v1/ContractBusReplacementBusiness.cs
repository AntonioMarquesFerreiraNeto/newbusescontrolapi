using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Requests.v1;
using BusesControl.Filters.Notification;
using Microsoft.AspNetCore.Http;

namespace BusesControl.Business.v1;

public class ContractBusReplacementBusiness(
    INotificationContext _notificationContext,
    IBusBusiness _busBusiness,
    IContractBusiness _contractBusiness
) : IContractBusReplacementBusiness
{
    private bool ValidateStartDateAndTerminateDate(DateOnly startDate, DateOnly terminateDate, ContractModel contract)
    {
        if (terminateDate < startDate)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.ContractBusReplacement.StartDateLessTerminateDate
            );
            return false;
        }

        if (startDate < contract.StartDate)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.ContractBusReplacement.StartDateLessContractStartDate
            );
            return false;
        }

        if (terminateDate > contract.TerminateDate)
        {
            _notificationContext.SetNotification(
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
        if (_notificationContext.HasNotification)
        {
            return false;
        }

        ValidateStartDateAndTerminateDate(request.StartDate, request.TerminateDate, contractRecord);
        if (_notificationContext.HasNotification)
        {
            return false;
        }

        if (request.BusId == contractRecord.BusId)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.ContractBusReplacement.BusInvalid
            );
            return false;
        }

        await _busBusiness.ValidateForContractBusReplacementAsync(request.BusId);
        if (_notificationContext.HasNotification)
        {
            return false;
        }

        return true;
    }
}
