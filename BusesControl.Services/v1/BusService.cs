﻿using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Enums.v1;
using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Requests.v1;
using BusesControl.Entities.Responses.v1;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using BusesControl.Persistence.UnitOfWork;
using BusesControl.Services.v1.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BusesControl.Services.v1;

public class BusService(
    IUnitOfWork _unitOfWork,
    INotificationContext _notificationContext,
    IBusBusiness _busBusiness,
    IColorBusiness _colorBusiness,
    IBusRepository _busRepository
) : IBusService
{
    private bool ValidateManufactureDate(DateOnly manufactureDate)
    {
        var dateNow = DateOnly.FromDateTime(DateTime.UtcNow);
        if (manufactureDate > dateNow)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Bus.ManufactureDateInvalid
            );
            return false;
        }

        return true;
    }

    public async Task<PaginationResponse<BusModel>> FindBySearchAsync(PaginationRequest request)
    {
        var records = await _busRepository.FindBySearchAsync(request.Page, request.PageSize, request.Search);
        var count = await _busRepository.CountBySearchAsync(request.Search);
        
        return new PaginationResponse<BusModel> 
        { 
            Response = records,
            TotalSize = count
        };
    }

    public async Task<BusModel> GetByIdAsync(Guid id)
    {
        var record = await _busRepository.GetByIdAsync(id);
        if (record is null)
        {
            _notificationContext.SetNotification(
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

        ValidateManufactureDate(request.ManufactureDate);
        if (_notificationContext.HasNotification)
        {
            return false;
        }

        await _colorBusiness.ValidateActiveAsync(request.ColorId);
        if (_notificationContext.HasNotification)
        {
            return false;
        }

        await _busBusiness.ExistsByRenavamOrLicensePlateOrChassisAsync(request.Renavam, request.LicensePlate, request.Chassi);
        if (_notificationContext.HasNotification)
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

        await _busRepository.AddAsync(record);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> UpdateAsync(Guid id, BusUpdateRequest request)
    {
        request.LicensePlate = request.LicensePlate.Replace("-", "");

        ValidateManufactureDate(request.ManufactureDate);
        if (_notificationContext.HasNotification)
        {
            return false;
        }

        var record = await _busRepository.GetByIdAsync(id);
        if (record is null)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Bus.NotFound
            );
            return false;
        }

        await _colorBusiness.ValidateActiveAsync(request.ColorId);
        if (_notificationContext.HasNotification)
        {
            return false;
        }

        await _busBusiness.ExistsByRenavamOrLicensePlateOrChassisAsync(request.Renavam, request.LicensePlate, request.Chassi, id);
        if (_notificationContext.HasNotification)
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
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Bus.NotFound
            );
            return false;
        }

        record.Status = record.Status != BusStatusEnum.Active ? BusStatusEnum.Active : BusStatusEnum.Inactive;
        if (record.Status == BusStatusEnum.Inactive)
        {
            record.Availability = AvailabilityEnum.Unavailable;
        }

        _busRepository.Update(record);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> ToggleAvailabilityAsync(Guid id)
    {
        var record = await _busRepository.GetByIdAsync(id);
        if (record is null)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Bus.NotFound
            );
            return false;
        }

        if (record.Status != BusStatusEnum.Active)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Bus.NotActive
            );
            return false;
        }

        record.Availability = record.Availability != AvailabilityEnum.Available ? AvailabilityEnum.Available : AvailabilityEnum.Unavailable;
        
        _busRepository.Update(record);
        await _unitOfWork.CommitAsync();

        return true;
    }
}