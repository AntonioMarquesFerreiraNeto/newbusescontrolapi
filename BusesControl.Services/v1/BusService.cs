using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Enums;
using BusesControl.Entities.Models;
using BusesControl.Entities.Request;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using BusesControl.Persistence.v1.UnitOfWork;
using BusesControl.Services.v1.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BusesControl.Services.v1;

public class BusService(
    IUnitOfWork _unitOfWork,
    INotificationApi _notificationApi,
    IBusBusiness _busBusiness,
    IColorBusiness _colorBusiness,
    IBusRepository _busRepository
) : IBusService
{
    public async Task<IEnumerable<BusModel>> FindBySearchAsync(int page, int pageSize, string? search = null)
    {
        var records = await _busRepository.FindBySearchAsync(page, pageSize, search);
        return records;
    }

    public async Task<BusModel> GetByIdAsync(Guid id)
    {
        var record = await _busRepository.GetByIdAsync(id);
        if (record is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Bus.NotFound
            );
            return default!;
        }

        return record;
    }

    public async Task<bool> CreateAsync(BusCreateRequest request)
    {
        request.LicensePlate = request.LicensePlate.Replace("-", "");

        await _colorBusiness.ValidateActiveAsync(request.ColorId);
        if (_notificationApi.HasNotification)
        {
            return false;
        }

        await _busBusiness.ExistsByRenavamOrLicensePlateOrChassisAsync(request.Renavam, request.LicensePlate, request.Chassi);
        if (_notificationApi.HasNotification)
        {
            return false;
        }

        var record = new BusModel
        {
            Brand = request.Brand,
            Name = request.Name,
            ManufactureDate = request.ManufactureDate,
            Renavam = request.Renavam,
            LicensePlate = request.LicensePlate.Replace("-", ""),
            Chassi = request.Chassi,
            SeatingCapacity = request.SeatingCapacity,
            ColorId = request.ColorId,
        };

        await _busRepository.CreateAsync(record);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> UpdateAsync(Guid id, BusUpdateRequest request)
    {
        request.LicensePlate = request.LicensePlate.Replace("-", "");

        var record = await _busRepository.GetByIdAsync(id);
        if (record is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Bus.NotFound
            );
            return false;
        }

        await _colorBusiness.ValidateActiveAsync(request.ColorId);
        if (_notificationApi.HasNotification)
        {
            return false;
        }

        await _busBusiness.ExistsByRenavamOrLicensePlateOrChassisAsync(request.Renavam, request.LicensePlate, request.Chassi, id);
        if (_notificationApi.HasNotification)
        {
            return false;
        }

        record.Brand = request.Brand;
        record.Name = request.Name;
        record.ManufactureDate = request.ManufactureDate;
        record.Renavam = request.Renavam;
        record.LicensePlate = request.LicensePlate;
        record.Chassi = request.Chassi;
        record.SeatingCapacity = request.SeatingCapacity;
        record.ColorId = request.ColorId;

        _busRepository.Update(record);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> ToggleActiveAsync(Guid id)
    {
        var record = await _busRepository.GetByIdAsync(id);
        if (record is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Bus.NotFound
            );
            return false;
        }

        record.Status = record.Status != BusStatusEnum.Active ? BusStatusEnum.Active : BusStatusEnum.Inactive;
        _busRepository.Update(record);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> ToggleAvailabilityAsync(Guid id)
    {
        var record = await _busRepository.GetByIdAsync(id);
        if (record is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Bus.NotFound
            );
            return false;
        }

        record.Availability = record.Availability != AvailabilityEnum.Available ? AvailabilityEnum.Available : AvailabilityEnum.Unavailable;
        
        _busRepository.Update(record);
        await _unitOfWork.CommitAsync();

        return true;
    }
}