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
    IContractRepository _contractRepository
) : ITerminationBusiness
{
    public async Task<ContractModel> GetForCreateAsync(Guid contractId, Guid customerId)
    {
        var contractRecord = await _contractRepository.GetByIdAndCustomerWithSettingPanelAsync(contractId, customerId);
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
}
