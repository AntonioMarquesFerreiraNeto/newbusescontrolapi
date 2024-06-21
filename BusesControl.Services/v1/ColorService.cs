using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Models;
using BusesControl.Entities.Requests;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using BusesControl.Persistence.v1.UnitOfWork;
using BusesControl.Services.v1.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BusesControl.Services.v1;

public class ColorService(
    IUnitOfWork _unitOfWork,
    INotificationApi _notificationApi,
    IColorBusiness _colorBusiness,
    IColorRepository _colorRepository
) : IColorService
{
    public async Task<ColorModel> GetByIdAsync(Guid id)
    {
        var record = await _colorRepository.GetByIdAsync(id);
        if (record is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Color.NotFound
            );
            return default!;
        }

        return record;
    }

    public async Task<IEnumerable<ColorModel>> FindBySearchAsync(int page, int pageSize, string? search = null)
    {
        var records = await _colorRepository.FindBySearchAsync(page, pageSize, search);
        return records;
    }

    public async Task<bool> CreateAsync(ColorCreateRequest request)
    {
        await _colorBusiness.ExistsAsync(request.Color);
        if (_notificationApi.HasNotification)
        {
            return false;
        }

        var record = new ColorModel 
        { 
            Color = request.Color
        };

        await _colorRepository.CreateAsync(record);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> UpdateAsync(Guid id, ColorUpdateRequest request)
    {
        var record = await _colorRepository.GetByIdAsync(id);
        if (record is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Color.NotFound
            );
            return false;
        }

        await _colorBusiness.ExistsAsync(request.Color);
        if (_notificationApi.HasNotification)
        {
            return false;
        }

        record.Color = request.Color;
        _colorRepository.Update(record);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> ToggleActiveAsync(Guid id)
    {
        var record = await _colorRepository.GetByIdAsync(id);
        if (record is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Color.NotFound
            );
            return default!;
        }

        record.Active = !record.Active;
        _colorRepository.Update(record);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var record = await _colorBusiness.GetForDeleteAsync(id);
        if (record is null)
        {
            return false;
        }

        _colorRepository.Remove(record);
        await _unitOfWork.CommitAsync();

        return true;
    }
}
