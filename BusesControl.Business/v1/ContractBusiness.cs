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
    ISettingsPanelRepository _settingsPanelRepository,
    IContractRepository _contractRepository,
    IBusRepository _busRepository,
    IEmployeeRepository _employeeRepository,
    INotificationApi _notificationApi
) : IContractBusiness
{
    public async Task<bool> ValidateTerminationDateAsync(DateTime terminateDate)
    {
        terminateDate = terminateDate.Date;
        var dateNow = DateTime.UtcNow.Date;

        if (dateNow >= terminateDate)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Contract.TerminationDateNotInFuture
            );
            return false;
        }

        var settingPanelRecord = await _settingsPanelRepository.GetByParentAsync(SettingsPanelParentEnum.Contract);

        if (settingPanelRecord?.LimitDateTermination is not null)
        {
            var dateLimit = dateNow.AddYears(settingPanelRecord.LimitDateTermination.Value);

            if (terminateDate > dateLimit)
            {
                _notificationApi.SetNotification(
                    statusCode: StatusCodes.Status404NotFound,
                    title: NotificationTitle.NotFound,
                    details: Message.Contract.TerminationDateExceedsLimit
                );
                return false;
            }
        }

        return true;
    }

    public async Task<bool> ValidateBusAndEmployeeVinculationAsync(Guid busId, Guid driverId)
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

        if (busRecord.Status == BusStatusEnum.Inactive)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Bus.NotActive
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

        var employeeRecord = await _employeeRepository.GetByIdAsync(driverId);
        if (employeeRecord is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Employee.NotFound
            );
            return default!;
        }

        if (employeeRecord.Status == EmployeeStatusEnum.Inactive)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Employee.NotActive
            );
            return default!;
        }

        if (employeeRecord.Type != EmployeeTypeEnum.Driver)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Contract.EmployeeNotDriver
            );
            return default!;
        }

        return true;
    }

    public async Task<ContractModel> GetForUpdateAsync(Guid id)
    {
        var contractRecord = await _contractRepository.GetByIdAsync(id);
        if (contractRecord is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Contract.NotFound
            );
            return default!;
        }

        if (contractRecord.Status != ContractStatusEnum.WaitingReview && contractRecord.Status != ContractStatusEnum.Denied)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Contract.InvalidEditRequest
            );
            return default!;
        }

        return contractRecord;
    }

    public async Task<ContractModel> GetForDeniedAsync(Guid id)
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

    public async Task<ContractModel> GetForWaitingReviewAsync(Guid id)
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

        if (record.Status != ContractStatusEnum.Denied)
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

    public async Task<ContractModel> GetForDeleteAsync(Guid id)
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

        if (record.Status != ContractStatusEnum.WaitingReview && record.Status != ContractStatusEnum.Denied)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Contract.InvalidRemoveRequest
            );
            return default!;
        }

        return record;
    }

    public bool ValidateDuplicateCustomersValidate(IEnumerable<Guid> customersId)
    {
        var duplicateExists = customersId.GroupBy(x => x).Any(x => x.Count() > 1);
        if (duplicateExists)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status409Conflict,
                title: NotificationTitle.Conflict,
                details: Message.Contract.DuplicateCustomers
            );
            return false;
        }

        return true;
    }
}
