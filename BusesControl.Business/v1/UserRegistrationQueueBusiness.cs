using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Enums.v1;
using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Requests.v1;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using Microsoft.AspNetCore.Http;

namespace BusesControl.Business.v1;

public class UserRegistrationQueueBusiness(
    INotificationContext _notificationContext,
    IEmployeeRepository _employeeRepository,
    IUserRegistrationQueueRepository _userRegistrationQueueRepository
) : IUserRegistrationQueueBusiness
{
    public async Task<EmployeeModel> GetForValidateForCreateAsync(Guid employeeId)
    {
        var employeeRecord = await _employeeRepository.GetByIdAsync(employeeId);
        if (employeeRecord is null)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Employee.NotFound
            );
            return default!;
        }

        if (employeeRecord.Type == EmployeeTypeEnum.Driver || employeeRecord.Status == EmployeeStatusEnum.Inactive)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.UserRegistration.RequestDenied
            );
            return default!;
        }

        var exists = await _userRegistrationQueueRepository.ExistsByEmployee(employeeId);
        if (exists)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.UserRegistration.Exists
            );
            return default!;
        }

        return employeeRecord;
    }

    public async Task<UserRegistrationQueueModel> GetForRegistrationUserStepCodeAsync(UserRegistrationStepCodeRequest request)
    {
        var record = await _userRegistrationQueueRepository.GetByEmployeeAttributesAsync(request.Email, request.Cpf, request.BirthDate);
        if (record is null)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.UserRegistration.NotFound
            );
            return default!;
        }

        if (record.Status != UserRegistrationQueueStatusEnum.Started && record.Status != UserRegistrationQueueStatusEnum.WaitingForPassword)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.UserRegistration.InvalidStepCode
            );
            return default!;
        }

        return record;
    }

    public async Task<UserRegistrationQueueModel> GetForRegistrationUserStepPasswordAsync(Guid userId)
    {
        var record = await _userRegistrationQueueRepository.GetByUserWithEmployeeAsync(userId);
        if (record is null)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.UserRegistration.NotFound
            );
            return default!;
        }

        if (record.Status != UserRegistrationQueueStatusEnum.WaitingForPassword)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.UserRegistration.InvalidStepPassword
            );
            return default!;
        }

        return record;
    }

    public async Task<UserRegistrationQueueModel> GetForDeleteAsync(Guid id)
    {
        var record = await _userRegistrationQueueRepository.GetByIdAsync(id);
        if (record is null)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.UserRegistration.NotFound
            );
            return default!;
        }

        if (record.Status == UserRegistrationQueueStatusEnum.Approved)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.UserRegistration.InvalidDelete
            );
            return default!;
        }

        return record;
    }

    public async Task<UserRegistrationQueueModel> GetForApproveAsync(Guid id)
    {
        var record = await _userRegistrationQueueRepository.GetByIdAsync(id);
        if (record is null)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.UserRegistration.NotFound
            );
            return default!;
        }

        if (record.Status != UserRegistrationQueueStatusEnum.Finished)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.UserRegistration.InvalidApprove
            );
            return default!;
        }

        return record;
    }
}
