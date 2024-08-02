using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Enums.v1;
using BusesControl.Entities.Models.v1;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using Microsoft.AspNetCore.Http;

namespace BusesControl.Business.v1;

public class FinancialBusiness(
    INotificationContext _notificationContext,
    IFinancialRepository _financialRepository
) : IFinancialBusiness
{
    public bool ValidateTerminationDate(SettingPanelModel settingPanelRecord, DateOnly terminateDate)
    {
        var dateNow = DateOnly.FromDateTime(DateTime.UtcNow.Date);

        if (dateNow >= terminateDate)
        {
            _notificationContext.SetNotification(
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
                _notificationContext.SetNotification(
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
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Financial.NotFound
            );
            return default!;
        }

        if (record.ContractId is not null)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Financial.InvalidInactive
            );
            return default!;
        }

        if (!record.Active)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Financial.IsInactive
            );
            return default!;
        }

        if (record.Type != FinancialTypeEnum.Revenue)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Financial.InvalidInactive
            );
            return default!;
        }

        return record;
    }

    public async Task<FinancialModel> GetForInactiveExpenseAsync(Guid id)
    {
        var record = await _financialRepository.GetByIdWithInvoicesExpenseAsync(id);
        if (record is null)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Financial.NotFound
            );
            return default!;
        }

        if (record.ContractId is not null)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Financial.InvalidInactive
            );
            return default!;
        }

        if (!record.Active)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Financial.IsInactive
            );
            return default!;
        }

        if (record.Type != FinancialTypeEnum.Expense)
        {
            _notificationContext.SetNotification(
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
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Financial.NotFound
            );
            return default!;
        }

        if (!record.Active)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Financial.IsInactive
            );
            return default!;
        }

        return record;
    }
}
