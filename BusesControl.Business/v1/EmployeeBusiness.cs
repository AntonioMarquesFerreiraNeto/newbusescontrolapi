using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Enums;
using BusesControl.Entities.Models;
using BusesControl.Entities.Request;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BusesControl.Business.v1;

public class EmployeeBusiness(
    INotificationApi _notificationApi,
    IEmployeeRepository _employeeRepository
) : IEmployeeBusiness
{
    public async Task<bool> ExistsByEmailOrPhoneNumberOrCpfAsync(string email, string phoneNumber, string cpf, Guid? id = null)
    {
        var exists = await _employeeRepository.ExistsByEmailOrPhoneNumberOrCpfAsync(email, phoneNumber, cpf, id);
        if (exists) 
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status409Conflict,
                title: NotificationTitle.Conflict,
                details: Message.Employee.Exists
            );
            return false;
        }

        return true;
    }

    public async Task<EmployeeModel> GetForToggleTypeAsync(Guid id, EmployeeToggleTypeRequest request)
    {
        var record = await _employeeRepository.GetByIdAsync(id);
        if (record is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Employee.NotFound
            );
            return default!;
        }

        if (record.Type == request.Type)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Employee.NoChangeNeeded
            );
            return default!;
        }

        return record;
    }

    public async Task<bool> ValidateForContractDriverReplacementAsync(Guid id)
    {
        var record = await _employeeRepository.GetByIdAsync(id);
        if (record is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Employee.NotFound
            );
            return false;
        }

        if (record.Type != EmployeeTypeEnum.Driver)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Employee.NotDriver
            );
            return false;
        }

        if (record.Status != EmployeeStatusEnum.Active)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Employee.NotActive
            );
            return false;
        }

        return true;
    }
}
