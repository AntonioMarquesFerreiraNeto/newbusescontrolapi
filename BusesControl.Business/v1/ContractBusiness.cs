﻿using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Enums.v1;
using BusesControl.Entities.Models.v1;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using Microsoft.AspNetCore.Http;

namespace BusesControl.Business.v1;

public class ContractBusiness(
    IContractRepository _contractRepository,
    ICustomerContractRepository _customerContractRepository,
    IBusRepository _busRepository,
    IEmployeeRepository _employeeRepository,
    INotificationContext _notificationContext
) : IContractBusiness
{
    public async Task<bool> ValidateBusAndEmployeeVinculationAsync(Guid busId, Guid driverId)
    {
        var busRecord = await _busRepository.GetByIdAsync(busId);
        if (busRecord is null)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Bus.NotFound
            );
            return default!;
        }

        if (busRecord.Status == BusStatusEnum.Inactive)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Bus.NotActive
            );
            return default!;
        }

        if (busRecord.Availability != AvailabilityEnum.Available)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Bus.NotAvailable
            );
            return default!;
        }

        var employeeRecord = await _employeeRepository.GetByIdAsync(driverId);
        if (employeeRecord is null)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Employee.NotFound
            );
            return default!;
        }

        if (employeeRecord.Status == EmployeeStatusEnum.Inactive)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Employee.NotActive
            );
            return default!;
        }

        if (employeeRecord.Type != EmployeeTypeEnum.Driver)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Contract.EmployeeNotDriver
            );
            return default!;
        }

        return true;
    }

    public async Task<CustomerContractModel> GetForGeneratedContractForCustomerAsync(Guid id, Guid customerId)
    {
        var customerContractRecord = await _customerContractRepository.GetByContractAndCustomerWithIncludesAsync(id, customerId);
        if (customerContractRecord is null)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.CustomerContract.NotFound
            );
            return default!;
        }

        if (!customerContractRecord.Contract.IsApproved)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.CustomerContract.NotPdfContract
            );
            return default!;
        }

        return customerContractRecord;
    }

    public async Task<CustomerContractModel> GetForGeneratedTerminationForCustomerAsync(Guid id, Guid customerId)
    {
        var customerContractRecord = await _customerContractRepository.GetByContractAndCustomerWithIncludesAsync(id, customerId);
        if (customerContractRecord is null)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.CustomerContract.NotFound
            );
            return default!;
        }

        if (customerContractRecord.Contract.Status != ContractStatusEnum.InProgress)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.CustomerContract.NotPdfTermination
            );
            return default!;
        }

        return customerContractRecord;
    }

    public async Task<ContractModel> GetForUpdateAsync(Guid id)
    {
        var contractRecord = await _contractRepository.GetByIdAsync(id);
        if (contractRecord is null)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Contract.NotFound
            );
            return default!;
        }

        if (contractRecord.Status != ContractStatusEnum.WaitingReview && contractRecord.Status != ContractStatusEnum.Denied)
        {
            _notificationContext.SetNotification(
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
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Contract.NotFound
            );
            return default!;
        }

        if (record.Status != ContractStatusEnum.WaitingReview)
        {
            _notificationContext.SetNotification(
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
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Contract.NotFound
            );
            return default!;
        }

        if (record.Status != ContractStatusEnum.Denied)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Contract.NotIsDenied
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
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Contract.NotFound
            );
            return default!;
        }

        if (record.Status != ContractStatusEnum.WaitingReview)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Contract.NotIsWaitingReview
            );
            return default!;
        }

        return record;
    }

    public async Task<ContractModel> GetForStartProgressAsync(Guid id)
    {
        var record = await _contractRepository.GetByIdWithCustomersContractAsync(id);
        if (record is null)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Contract.NotFound
            );
            return default!;
        }

        if (!record.IsApproved)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Contract.NotIsApproved
            );
            return default!;
        }

        if (record.Status != ContractStatusEnum.WaitingSignature)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Contract.NotIsWaitingSignature
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
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Contract.NotFound
            );
            return default!;
        }

        if (record.Status != ContractStatusEnum.WaitingReview && record.Status != ContractStatusEnum.Denied)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Contract.InvalidRemoveRequest
            );
            return default!;
        }

        return record;
    }

    public async Task<ContractModel> GetForContractBusOrDriverReplacementAsync(Guid contractId)
    {
        var contractRecord = await _contractRepository.GetByIdWithSettingPanelAsync(contractId);
        if (contractRecord is null)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Contract.NotFound
            );
            return default!;
        }

        if (contractRecord.Status != ContractStatusEnum.InProgress)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Contract.NotInProgress
            );
            return default!;
        }

        return contractRecord;
    }

    public bool ValidateTerminationDate(SettingPanelModel settingPanelRecord, DateOnly terminateDate)
    {
        var dateNow = DateOnly.FromDateTime(DateTime.UtcNow.Date);

        if (dateNow >= terminateDate)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Contract.TerminationDateNotInFuture
            );
            return false;
        }

        if (settingPanelRecord.LimitDateTerminate is not null)
        {
            var dateLimit = dateNow.AddYears(settingPanelRecord.LimitDateTerminate.Value);

            if (terminateDate > dateLimit)
            {
                _notificationContext.SetNotification(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: NotificationTitle.BadRequest,
                    details: Message.Contract.TerminationDateExceedsLimit
                );
                return false;
            }
        }

        return true;
    }

    public bool ValidateDuplicateCustomersValidate(IEnumerable<Guid> customersId)
    {
        var duplicateExists = customersId.GroupBy(x => x).Any(x => x.Count() > 1);
        if (duplicateExists)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status409Conflict,
                title: NotificationTitle.Conflict,
                details: Message.Contract.DuplicateCustomers
            );
            return false;
        }

        return true;
    }
}
