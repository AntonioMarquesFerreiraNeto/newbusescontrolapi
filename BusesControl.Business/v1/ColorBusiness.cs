﻿using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Models.v1;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using Microsoft.AspNetCore.Http;

namespace BusesControl.Business.v1;

public class ColorBusiness(
    INotificationContext _notificationContext,
    IBusRepository _busRepository,
    IColorRepository _colorRepository
) : IColorBusiness
{
    public async Task<bool> ValidateActiveAsync(Guid id)
    {
        var record = await _colorRepository.GetByIdAsync(id);
        if (record is null) 
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Color.NotFound
            );
            return false;
        }

        if (!record.Active)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Color.NotActive
            );
            return false;
        }

        return true;
    }

    public async Task<bool> ExistsAsync(string color, Guid? id = null)
    {
        var exists = await _colorRepository.ExistsAsync(color, id: id);
        if (exists)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status409Conflict,
                title: NotificationTitle.Conflict,
                details: Message.Color.Exists
            );
            return false;
        }

        return true;
    }

    public async Task<ColorModel> GetForDeleteAsync(Guid id)
    {
        var record = await _colorRepository.GetByIdAsync(id);
        if (record is null)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Color.NotFound
            );
            return default!;
        }

        var exists = await _busRepository.ExistsByColorAsync(id);
        if (exists)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Color.ExistsInBus
            );
            return default!;
        }

        return record;
    }
}
