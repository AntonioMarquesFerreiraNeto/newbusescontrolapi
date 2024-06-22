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
    ISettingsPanelRepository _settingsPanelRepository
) : ISettingsPanelBusiness
{
    public async Task<bool> ExistsByParentAsync(SettingsPanelParentEnum parent)
    {
        var exists = await _settingsPanelRepository.ExistsByParent(parent);
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

        var exists = await _settingsPanelRepository.ExistsByParent(parent, id);
        if (exists)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status409Conflict,
                title: NotificationTitle.Conflict,
                details: Message.SettingsPanel.Exists
            );
            return default!;
        }

        return record;
    }
}
