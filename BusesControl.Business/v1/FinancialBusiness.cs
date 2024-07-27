using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Enums;
using BusesControl.Entities.Models;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BusesControl.Business.v1;

public class FinancialBusiness(
    INotificationApi _notificationApi,
    IFinancialRepository _financialRepository
) : IFinancialBusiness
{
    public bool ValidateTerminationDate(SettingPanelModel settingPanelRecord, DateOnly terminateDate)
    {
        var dateNow = DateOnly.FromDateTime(DateTime.UtcNow.Date);

        if (dateNow >= terminateDate)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Contract.TerminationDateNotInFuture
            );
            return false;
        }

        if (settingPanelRecord.LimitDateTerminate is not null)
        {
            var dateLimit = dateNow.AddYears(settingPanelRecord.LimitDateTerminate.Value);

            if (terminateDate > dateLimit)
            {
                _notificationApi.SetNotification(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: NotificationTitle.BadRequest,
                    details: Message.Financial.TerminationDateExceedsLimit
                );
                return false;
            }
        }

        return true;
    }

    public async Task<FinancialModel> GetForInactiveRevenueAsync(Guid id)
    {
        var record = await _financialRepository.GetByIdWithInvoicesAsync(id);
        if (record is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Financial.NotFound
            );
            return default!;
        }

        if (record.ContractId is not null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Financial.InvalidInactive
            );
            return default!;
        }

        if (!record.Active)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Financial.IsInactive
            );
            return default!;
        }

        if (record.Type != FinancialTypeEnum.Revenue)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Financial.InvalidInactive
            );
            return default!;
        }

        return record;
    }

    public async Task<FinancialModel> GetForUpdateDetailsAsync(Guid id)
    {
        var record = await _financialRepository.GetByIdAsync(id);
        if (record is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Financial.NotFound
            );
            return default!;
        }

        if (!record.Active)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Financial.IsInactive
            );
            return default!;
        }

        return record;
    }
}
