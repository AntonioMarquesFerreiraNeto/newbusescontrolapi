using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Requests.v1;
using BusesControl.Entities.Responses.v1;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using BusesControl.Persistence.UnitOfWork;
using BusesControl.Services.v1.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BusesControl.Services.v1;

public class ColorService(
    IUnitOfWork _unitOfWork,
    INotificationContext _notificationContext,
    IColorBusiness _colorBusiness,
    IColorRepository _colorRepository
) : IColorService
{
    public async Task<PaginationResponse<ColorModel>> FindBySearchAsync(PaginationRequest request)
    {
        var records = await _colorRepository.FindBySearchAsync(request.Page, request.PageSize, request.Search);
        var count = await _colorRepository.CountBySearchAsync(request.Search);

        return new PaginationResponse<ColorModel>
        {
            Response = records,
            TotalSize = count
        };
    }

    public async Task<ColorModel> GetByIdAsync(Guid id)
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

        return record;
    }

    public async Task<bool> CreateAsync(ColorCreateRequest request)
    {
        await _colorBusiness.ExistsAsync(request.Color);
        if (_notificationContext.HasNotification)
        {
            return false;
        }

        var record = new ColorModel 
        { 
            Color = request.Color
        };

        await _colorRepository.AddAsync(record);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> UpdateAsync(Guid id, ColorUpdateRequest request)
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

        await _colorBusiness.ExistsAsync(request.Color);
        if (_notificationContext.HasNotification)
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
            _notificationContext.SetNotification(
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
