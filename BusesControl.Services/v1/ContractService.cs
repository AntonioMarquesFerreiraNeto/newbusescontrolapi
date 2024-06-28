﻿using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Enums;
using BusesControl.Entities.Models;
using BusesControl.Entities.Requests;
using BusesControl.Entities.Response;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using BusesControl.Persistence.v1.UnitOfWork;
using BusesControl.Services.v1.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BusesControl.Services.v1;

public class ContractService(
    IUnitOfWork _unitOfWork,
    INotificationApi _notificationApi,
    IUserService _userService,
    ICustomerContractService _customerContractService,
    IContractBusiness _contractBusiness,
    IContractRepository _contractRepository
) : IContractService
{
    private static int CalculateMonths(DateTime startDate, DateTime endDate)
    {
        int startYear = startDate.Year;
        int startMonth = startDate.Month;

        int endYear = endDate.Year;
        int endMonth = endDate.Month;

        int monthDifference = (endYear - startYear) * 12 + (endMonth - startMonth);
        
        return monthDifference;
    }

    public async Task<ContractModel?> GetByIdAsync(Guid id)
    {
        var record = await _contractRepository.GetByIdWithIncludesAsync(id);
        if (record is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Contract.NotFound
            );
            return default!;
        }

        return record;
    }

    public async Task<IEnumerable<ContractModel>> FindByOptionalStatusAsync(int page, int pageSize, ContractStatusEnum? status)
    {
        var records = await _contractRepository.FindByOptionalStatusAsync(page, pageSize, status);
        return records;
    }

    public async Task<bool> CreateAsync(ContractCreateRequest request)
    {
        _contractBusiness.ValidateDuplicateCustomersValidate(request.CustomersId);
        if (_notificationApi.HasNotification)
        {
            return false;
        }
        
        await _contractBusiness.ValidateTerminationDateAsync(request.TerminateDate);
        if (_notificationApi.HasNotification)
        {
            return false;
        }

        await _contractBusiness.ValidateBusAndEmployeeVinculationAsync(request.BusId, request.DriverId);
        if (_notificationApi.HasNotification)
        {
            return false;
        }

        _unitOfWork.BeginTransaction();

        var record = new ContractModel
        {
            BusId = request.BusId,
            DriverId = request.DriverId,
            TotalPrice = request.TotalPrice,
            PaymentMethod = request.PaymentMethod,
            Details = request.Details,
            TerminateDate = request.TerminateDate
        };

        await _contractRepository.CreateAsync(record);
        await _unitOfWork.CommitAsync();

        await _customerContractService.CreateForContractAsync(request.CustomersId, record.Id);
        if (_notificationApi.HasNotification)
        {
            return false;
        }

        await _unitOfWork.CommitAsync(true);

        return true;
    }

    public async Task<bool> UpdateAsync(Guid id, ContractUpdateRequest request)
    {
        _contractBusiness.ValidateDuplicateCustomersValidate(request.CustomersId);
        if (_notificationApi.HasNotification)
        {
            return false;
        }

        await _contractBusiness.ValidateTerminationDateAsync(request.TerminateDate);
        if (_notificationApi.HasNotification)
        {
            return false;
        }

        await _contractBusiness.ValidateBusAndEmployeeVinculationAsync(request.BusId, request.DriverId);
        if (_notificationApi.HasNotification)
        {
            return false;
        }

        var record = await _contractBusiness.GetForUpdateAsync(id);
        if (_notificationApi.HasNotification)
        {
            return false;
        }

        _unitOfWork.BeginTransaction();

        record.BusId = request.BusId;
        record.DriverId = request.DriverId;
        record.TotalPrice = request.TotalPrice;
        record.PaymentMethod = request.PaymentMethod;
        record.Details = request.Details;
        record.TerminateDate = request.TerminateDate;
        record.UpdatedAt = DateTime.UtcNow;

        _contractRepository.Update(record);
        await _unitOfWork.CommitAsync();

        await _customerContractService.UpdateForContractAsync(request.CustomersId, record.Id);
        if (_notificationApi.HasNotification)
        {
            return false;
        }

        await _unitOfWork.CommitAsync(true);

        return true;
    }

    public async Task<bool> DeniedAsync(Guid id)
    {
        var record = await _contractBusiness.GetForDeniedAsync(id);
        if (_notificationApi.HasNotification)
        {
            return false;
        }

        record.UpdatedAt = DateTime.UtcNow;
        record.Status = ContractStatusEnum.Denied;
        _contractRepository.Update(record);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> WaitingReviewAsync(Guid id)
    {
        var record = await _contractBusiness.GetForWaitingReviewAsync(id);
        if (_notificationApi.HasNotification)
        {
            return false;
        }

        record.UpdatedAt = DateTime.UtcNow;
        record.Status = ContractStatusEnum.WaitingReview;
        _contractRepository.Update(record);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<SuccessResponse> ApproveAsync(Guid id)
    {
        var record = await _contractBusiness.GetForApproveAsync(id);
        if (_notificationApi.HasNotification)
        {
            return default!;
        }

        var startDate = DateTime.UtcNow;
        var installmentsCount = record.PaymentMethod == ContractPaymentMethodEnum.Single ? 1 : CalculateMonths(startDate, record.TerminateDate);

        record.UpdatedAt = DateTime.UtcNow;
        record.StartDate = startDate;
        record.Status = ContractStatusEnum.InProgress;
        record.InstallmentsCount = installmentsCount;
        record.IsApproved = true;
        record.ApproverId = _userService.FindAuthenticatedUser().EmployeeId;

        _contractRepository.Update(record);
        await _unitOfWork.CommitAsync();

        //TODO: chamar service de Financial para tratar as questões financeiras do contrato, e gerar as folhas de pagamento em FinancialInstallment.

        return new SuccessResponse(Message.Contract.SuccessfullyApproved);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var record = await _contractBusiness.GetForDeleteAsync(id);
        if (_notificationApi.HasNotification)
        {
            return false;
        }

        _contractRepository.Delete(record);
        await _unitOfWork.CommitAsync();

        return true;
    }
}
