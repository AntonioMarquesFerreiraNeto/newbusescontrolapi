using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Enums;
using BusesControl.Entities.Models;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BusesControl.Business.v1;

public class SettingsPanelBusiness(
    INotificationApi _notificationApi,
    IContractRepository _contractRepository,
    ISettingsPanelRepository _settingsPanelRepository
) : ISettingsPanelBusiness
{
    public async Task<bool> ExistsByParentAsync(SettingsPanelParentEnum parent)
    {
        var exists = await _settingsPanelRepository.ExistsByParentExceptionContract(parent);
        if (exists)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status409Conflict,
                title: NotificationTitle.Conflict,
                details: Message.SettingsPanel.Exists
            );
            return false;
        }

        return true;
    }

    public async Task<SettingsPanelModel> GetForUpdateAsync(Guid id, SettingsPanelParentEnum parent)
    {
        var record = await _settingsPanelRepository.GetByIdAsync(id);
        if (record is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.SettingsPanel.NotFound
            );
            return default!;
        }

        var exists = await _settingsPanelRepository.ExistsByParentExceptionContract(parent, id);
        if (exists)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status409Conflict,
                title: NotificationTitle.Conflict,
                details: Message.SettingsPanel.Exists
            );
            return default!;
        }

        var contractInIsApprovedExists = await _contractRepository.ExistsInIsApprovedBySettingsPanelAsync(id);
        if (contractInIsApprovedExists)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.SettingsPanel.NotUpdate
            );
            return default!;
        }

        return record;
    }

    public async Task<SettingsPanelModel> GetForDeleteAsync(Guid id)
    {
        var record = await _settingsPanelRepository.GetByIdAsync(id);
        if (record is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.SettingsPanel.NotFound
            );
            return default!;
        }

        var existsInContract = await _contractRepository.ExistsBySettingsPanelAsync(id);
        if (existsInContract)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.SettingsPanel.NotDelete
            );
            return default!;
        }

        return record;
    }

    public async Task<SettingsPanelModel> GetForCreateOrUpdateContractAsync(Guid settingsPanelId)
    {
        var settingPanelRecord = await _settingsPanelRepository.GetByIdAsync(settingsPanelId);
        if (settingPanelRecord is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.SettingsPanel.NotFound
            );
            return default!;
        }

        if (settingPanelRecord.Parent != SettingsPanelParentEnum.Contract || !settingPanelRecord.Active)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.SettingsPanel.NotDestine
            );
            return default!;
        }

        return settingPanelRecord;
    }
}
