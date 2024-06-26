﻿using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Enums;
using BusesControl.Entities.Models;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BusesControl.Business.v1;

public class SettingPanelBusiness(
    INotificationApi _notificationApi,
    IContractRepository _contractRepository,
    ISettingPanelRepository _settingPanelRepository
) : ISettingPanelBusiness
{
    public async Task<bool> ExistsByParentAsync(SettingPanelParentEnum parent)
    {
        var exists = await _settingPanelRepository.ExistsByParentExceptionContract(parent);
        if (exists)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status409Conflict,
                title: NotificationTitle.Conflict,
                details: Message.SettingPanel.Exists
            );
            return false;
        }

        return true;
    }

    public async Task<SettingPanelModel> GetForUpdateAsync(Guid id, SettingPanelParentEnum parent)
    {
        var record = await _settingPanelRepository.GetByIdAsync(id);
        if (record is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.SettingPanel.NotFound
            );
            return default!;
        }

        var exists = await _settingPanelRepository.ExistsByParentExceptionContract(parent, id);
        if (exists)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status409Conflict,
                title: NotificationTitle.Conflict,
                details: Message.SettingPanel.Exists
            );
            return default!;
        }

        var contractInIsApprovedExists = await _contractRepository.ExistsInIsApprovedBySettingPanelAsync(id);
        if (contractInIsApprovedExists)
        {
            _notificationApi.SetNotification(
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
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.SettingPanel.NotFound
            );
            return default!;
        }

        var existsInContract = await _contractRepository.ExistsBySettingPanelAsync(id);
        if (existsInContract)
        {
            _notificationApi.SetNotification(
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
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.SettingPanel.NotFound
            );
            return default!;
        }

        if (settingPanelRecord.Parent != SettingPanelParentEnum.Contract || !settingPanelRecord.Active)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.SettingPanel.NotDestine
            );
            return default!;
        }

        return settingPanelRecord;
    }
}
