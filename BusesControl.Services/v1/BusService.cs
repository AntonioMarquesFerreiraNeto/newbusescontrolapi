using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons.Message;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Models;
using BusesControl.Entities.Request;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using BusesControl.Services.v1.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BusesControl.Services.v1;

public class BusService( 
    INotificationApi _notificationApi,
    IBusBusiness _busBusiness,
    IBusRepository _busRepository
) : IBusService
{
    public async Task<BusModel> GetByIdAsync(Guid id)
    {
        var record = await _busRepository.GetByIdAsync(id);
        if (record is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: SupportMessage.Bus.NotFound
            );
            return default!;
        }

        return record;
    }

    public async Task<bool> CreateAsync(BusCreateRequest request)
    {
        request.LicensePlate = request.LicensePlate.Replace("-", "");

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
            Color = request.Color,
        };

        await _busRepository.CreateAsync(record);

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
                details: SupportMessage.Bus.NotFound
            );
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
        record.Color = request.Color;
        
        await _busRepository.UpdateAsync(record);

        return true;
    }
}
