using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Requests.v1;
using BusesControl.Filters.Notification;
using Microsoft.AspNetCore.Http;

namespace BusesControl.Business.v1;

public class ContractDriverReplacementBusiness(
    INotificationContext _notificationContext,
    IContractBusiness _contractBusiness,
    IEmployeeBusiness _employeeBusiness
) : IContractDriverReplacementBusiness
{
    private bool ValidateStartDateAndTerminateDate(DateOnly startDate, DateOnly terminateDate, ContractModel contract)
    {
        if (terminateDate < startDate)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.ContractDriverReplacement.StartDateLessTerminateDate
            );
            return false;
        }

        if (startDate < contract.StartDate)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.ContractDriverReplacement.StartDateLessContractStartDate
            );
            return false;
        }

        if (terminateDate > contract.TerminateDate)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.ContractDriverReplacement.TerminateDateGreaterContractTerminateDate
            );
            return false;
        }

        return true;
    }

    public async Task<bool> ValidateForCreateAsync(Guid contractId, ContractDriverReplacementCreateRequest request)
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

        if (request.DriverId == contractRecord.DriverId)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.ContractDriverReplacement.DriverInvalid
            );
            return false;
        }

        await _employeeBusiness.ValidateForContractDriverReplacementAsync(request.DriverId);
        if (_notificationContext.HasNotification)
        {
            return false;
        }

        return true;
    }
}
