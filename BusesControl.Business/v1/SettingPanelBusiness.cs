using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Enums.v1;
using BusesControl.Entities.Models.v1;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using Microsoft.AspNetCore.Http;

namespace BusesControl.Business.v1;

public class SettingPanelBusiness(
    INotificationContext _notificationContext,
    IContractRepository _contractRepository,
    IFinancialRepository _financialRepository,
    ISettingPanelRepository _settingPanelRepository
) : ISettingPanelBusiness
{
    public async Task<SettingPanelModel> GetForUpdateAsync(Guid id, SettingPanelParentEnum parent)
    {
        var record = await _settingPanelRepository.GetByIdAsync(id);
        if (record is null)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.SettingPanel.NotFound
            );
            return default!;
        }

        var contractInIsApprovedExists = await _contractRepository.ExistsInIsApprovedBySettingPanelAsync(id);
        if (contractInIsApprovedExists)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.SettingPanel.NotUpdate
            );
            return default!;
        }

        return record;
    }

    public async Task<SettingPanelModel> GetForDeleteAsync(Guid id)
    {
        var record = await _settingPanelRepository.GetByIdAsync(id);
        if (record is null)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.SettingPanel.NotFound
            );
            return default!;
        }

        var existsInContract = await _contractRepository.ExistsBySettingPanelAsync(id);
        if (existsInContract)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.SettingPanel.NotDelete
            );
            return default!;
        }

        var existsInFinancial = await _financialRepository.ExistsBySettingPanelAsync(id);
        if (existsInFinancial)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.SettingPanel.NotDelete
            );
            return default!;
        }

        return record;
    }

    public async Task<SettingPanelModel> GetForCreateOrUpdateContractAsync(Guid settingPanelId)
    {
        var settingPanelRecord = await _settingPanelRepository.GetByIdAsync(settingPanelId);
        if (settingPanelRecord is null)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.SettingPanel.NotFound
            );
            return default!;
        }

        if (settingPanelRecord.Parent != SettingPanelParentEnum.Contract || !settingPanelRecord.Active)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.SettingPanel.NotDestine
            );
            return default!;
        }

        return settingPanelRecord;
    }

    public async Task<SettingPanelModel> GetForCreateFinancialRevenueAsync(Guid settingPanelId)
    {
        var settingPanelRecord = await _settingPanelRepository.GetByIdAsync(settingPanelId);
        if (settingPanelRecord is null)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.SettingPanel.NotFound
            );
            return default!;
        }

        if (settingPanelRecord.Parent != SettingPanelParentEnum.Revenue || !settingPanelRecord.Active)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.SettingPanel.NotDestine
            );
            return default!;
        }

        return settingPanelRecord;
    }

    public async Task<SettingPanelModel> GetForCreateFinancialExpenseAsync(Guid settingPanelId)
    {
        var settingPanelRecord = await _settingPanelRepository.GetByIdAsync(settingPanelId);
        if (settingPanelRecord is null)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.SettingPanel.NotFound
            );
            return default!;
        }

        if (settingPanelRecord.Parent != SettingPanelParentEnum.Expense || !settingPanelRecord.Active)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.SettingPanel.NotDestine
            );
            return default!;
        }

        return settingPanelRecord;
    }
}
