using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Enums;
using BusesControl.Entities.Models;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BusesControl.Business.v1;

public class TerminationBusiness(
    INotificationApi _notificationApi,
    IContractRepository _contractRepository,
    ICustomerContractRepository _customerContractRepository
) : ITerminationBusiness
{
    public async Task<ContractModel> GetContractForCreateAsync(Guid contractId)
    {
        var contractRecord = await _contractRepository.GetByIdWithSettingPanelAsync(contractId);
        if (contractRecord is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Contract.NotFound
            );
            return default!;
        }

        if (contractRecord.Status != ContractStatusEnum.InProgress)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Contract.NotInProgress
            );
            return default!;
        }

        return contractRecord;
    }

    public async Task<CustomerContractModel> GetCustomerContractForCreateAsync(Guid contractId, Guid customerId)
    {
        var customerContractRecord = await _customerContractRepository.GetByContractAndCustomerAsync(contractId, customerId);
        if (customerContractRecord is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.CustomerContract.NotFound
            );
            return default!;
        }

        if (!customerContractRecord.ProcessTermination)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Termination.NotProcess
            );
            return default!;
        }

        if (!customerContractRecord.Active)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Termination.NotActive
            );
            return default!;
        }

        return customerContractRecord;
    }
}
