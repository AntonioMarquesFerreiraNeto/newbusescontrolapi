using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Models;
using BusesControl.Entities.Requests;
using BusesControl.Filters.Notification;
using Microsoft.AspNetCore.Http;

namespace BusesControl.Business.v1;

public class ContractDriverReplacementBusiness(
    INotificationApi _notificationApi,
    IContractBusiness _contractBusiness,
    IEmployeeBusiness _employeeBusiness
) : IContractDriverReplacementBusiness
{
    private bool ValidateStartDateAndTerminateDate(DateOnly startDate, DateOnly terminateDate, ContractModel contract)
    {
        if (terminateDate < startDate)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.ContractDriverReplacement.StartDateLessTerminateDate
            );
            return false;
        }

        if (startDate < contract.StartDate)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.ContractDriverReplacement.StartDateLessContractStartDate
            );
            return false;
        }

        if (terminateDate > contract.TerminateDate)
        {
            _notificationApi.SetNotification(
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
        if (_notificationApi.HasNotification)
        {
            return false;
        }

        ValidateStartDateAndTerminateDate(request.StartDate, request.TerminateDate, contractRecord);
        if (_notificationApi.HasNotification)
        {
            return false;
        }

        if (request.DriverId == contractRecord.DriverId)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.ContractDriverReplacement.DriverInvalid
            );
            return false;
        }

        await _employeeBusiness.ValidateForContractDriverReplacementAsync(request.DriverId);
        if (_notificationApi.HasNotification)
        {
            return false;
        }

        return true;
    }
}
