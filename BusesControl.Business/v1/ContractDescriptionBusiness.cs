using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Models;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BusesControl.Business.v1;

public class ContractDescriptionBusiness(
    INotificationApi _notificationApi,
    IContractRepository _contractRepository,
    IContractDescriptionRepository _contractDescriptionRepository
) : IContractDescriptionBusiness
{
    public async Task<ContractDescriptionModel> GetForUpdateAsync(Guid id)
    {
        var record = await _contractDescriptionRepository.GetByIdAsync(id);
        if (record is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.ContractDescription.NotFound
            );
            return default!;
        }

        var usingInContractApproved = await _contractRepository.ExistsInIsApprovedByContractDescriptionAsync(id);
        if (usingInContractApproved)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.ContractDescription.NotUpdate
            );
            return default!;
        }

        return record;
    }

    public async Task<ContractDescriptionModel> GetForDeleteAsync(Guid id)
    {
        var record = await _contractDescriptionRepository.GetByIdAsync(id);
        if (record is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.ContractDescription.NotFound
            );
            return default!;
        }

        var usingInContract = await _contractRepository.ExistsByContractDescriptionAsync(id);
        if (usingInContract)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.ContractDescription.NotDelete
            );
            return default!;
        }

        return record;
    }
}
