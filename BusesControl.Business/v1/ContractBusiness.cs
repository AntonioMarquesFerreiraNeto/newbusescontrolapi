using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Enums;
using BusesControl.Entities.Models;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BusesControl.Business.v1;

public class ContractBusiness(
    IContractRepository _contractRepository,
    IBusRepository _busRepository,
    IEmployeeRepository _employeeRepository,
    INotificationApi _notificationApi
) : IContractBusiness
{
    public async Task<bool> ValidateForCreateAsync(Guid busId, Guid driverId)
    {
        var busRecord = await _busRepository.GetByIdAsync(busId);
        if (busRecord is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Bus.NotFound
            );
            return default!;
        }

        if (busRecord.Availability != AvailabilityEnum.Available)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Bus.NotAvailable
            );
            return default!;
        }

        var employeeRecord = await _employeeRepository.GetByIdAsync(busId);
        if (employeeRecord is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Employee.NotFound
            );
            return default!;
        }

        return true;
    }

    public async Task<ContractModel> GetForApproveAsync(Guid id)
    {
        var record = await _contractRepository.GetByIdAsync(id);
        if (record is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Contract.NotFound
            );
            return default!;
        }

        if (record.Status != ContractStatusEnum.WaitingReview)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Contract.NotIsWaitingReview
            );
            return default!;
        }

        return record;
    }
}
