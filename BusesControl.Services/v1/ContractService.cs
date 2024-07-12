using AutoMapper;
using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Enums;
using BusesControl.Entities.Models;
using BusesControl.Entities.Requests;
using BusesControl.Entities.Response;
using BusesControl.Entities.Responses;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using BusesControl.Persistence.v1.UnitOfWork;
using BusesControl.Services.v1.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BusesControl.Services.v1;

public class ContractService(
    IMapper _mapper,
    IUnitOfWork _unitOfWork,
    INotificationApi _notificationApi,
    IUserService _userService,
    IGenerationPdfService _generationPdfService,
    ICustomerContractService _customerContractService,
    IContractDescriptionService _contractDescriptionService,
    IFinancialService _financialService,
    IContractBusiness _contractBusiness,
    IContractRepository _contractRepository,
    ISettingPanelBusiness _settingPanelBusiness
) : IContractService
{
    private static int CalculateMonths(DateOnly startDate, DateOnly endDate)
    {
        int startYear = startDate.Year;
        int startMonth = startDate.Month;

        int endYear = endDate.Year;
        int endMonth = endDate.Month;

        int monthDifference = (endYear - startYear) * 12 + (endMonth - startMonth);
        
        return monthDifference;
    }

    private async Task<string> GenerateReferenceUniqueAsync()
    {
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var reference = "#";
        var random = new Random();
        var existsReference = true;

        while (existsReference)
        {
            for (int c = 0; c < 7; c++)
            {
                reference += chars[random.Next(chars.Length)];
            }
            existsReference = await _contractRepository.ExistsByReferenceAsync(reference);
        }

        return reference;
    }

    public async Task<ContractResponse> GetByIdAsync(Guid id)
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

        return _mapper.Map<ContractResponse>(record);
    }

    public async Task<PdfCoResponse> GetGeneratedContractForCustomerAsync(Guid id, Guid customerId)
    {
        var customerContractRecord = await _contractBusiness.GetForGeneratedContractForCustomerAsync(id, customerId);
        if (_notificationApi.HasNotification)
        {
            return default!;
        }

        var response = await _generationPdfService.GeneratePdfFromTemplateAsync(customerContractRecord);
        if (_notificationApi.HasNotification)
        {
            return default!;
        }

        return response;
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

        await _contractDescriptionService.ExistsAsync(request.ContractDescriptionId);
        if (_notificationApi.HasNotification)
        {
            return false;
        }

        var settingPanelRecord = await _settingPanelBusiness.GetForCreateOrUpdateContractAsync(request.SettingPanelId);
        if (_notificationApi.HasNotification)
        {
            return false;
        }

        _contractBusiness.ValidateTerminationDate(settingPanelRecord, request.TerminateDate);
        if (_notificationApi.HasNotification)
        {
            return false;
        }

        await _contractBusiness.ValidateBusAndEmployeeVinculationAsync(request.BusId, request.DriverId);
        if (_notificationApi.HasNotification)
        {
            return false;
        }

        var reference = await GenerateReferenceUniqueAsync();

        _unitOfWork.BeginTransaction();

        var record = new ContractModel
        {
            Reference = reference,
            BusId = request.BusId,
            DriverId = request.DriverId,
            SettingPanelId = request.SettingPanelId,
            ContractDescriptionId = request.ContractDescriptionId,
            TotalPrice = request.TotalPrice,
            PaymentType = request.PaymentType,
            Details = request.Details,
            TerminateDate = request.TerminateDate,
            CustomersCount = request.CustomersId.Count()
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

        await _contractDescriptionService.ExistsAsync(request.ContractDescriptionId);
        if (_notificationApi.HasNotification)
        {
            return false;
        }

        var settingPanelRecord = await _settingPanelBusiness.GetForCreateOrUpdateContractAsync(request.SettingPanelId);
        if (_notificationApi.HasNotification)
        {
            return false;
        }

        _contractBusiness.ValidateTerminationDate(settingPanelRecord, request.TerminateDate);
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
        record.SettingPanelId = request.SettingPanelId;
        record.ContractDescriptionId = request.ContractDescriptionId;
        record.TotalPrice = request.TotalPrice;
        record.PaymentType = request.PaymentType;
        record.Details = request.Details;
        record.TerminateDate = request.TerminateDate;
        record.UpdatedAt = DateTime.UtcNow;
        record.CustomersCount = request.CustomersId.Count();

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

        var startDate = DateTime.UtcNow.AddDays(2);
        var installmentsCount = record.PaymentType == PaymentTypeEnum.Single ? 1 : CalculateMonths(DateOnly.FromDateTime(startDate), record.TerminateDate);

        record.UpdatedAt = DateTime.UtcNow;
        record.StartDate = DateOnly.FromDateTime(startDate);
        record.Status = ContractStatusEnum.WaitingSignature;
        record.InstallmentsCount = installmentsCount;
        record.IsApproved = true;
        record.ApproverId = _userService.FindAuthenticatedUser().EmployeeId;

        _contractRepository.Update(record);
        await _unitOfWork.CommitAsync();

        return new SuccessResponse(Message.Contract.SuccessfullyApproved);
    }

    public async Task<SuccessResponse> StartProgressAsync(Guid id)
    {
        var record = await _contractBusiness.GetForStartProgressAsync(id);
        if (_notificationApi.HasNotification)
        {
            return default!;
        }

        _unitOfWork.BeginTransaction();

        await _financialService.CreateForContractAsync(record);
        if (_notificationApi.HasNotification)
        {
            return default!;
        }

        record.UpdatedAt = DateTime.UtcNow;
        record.Status = ContractStatusEnum.InProgress;
        _contractRepository.Update(record);
        await _unitOfWork.CommitAsync();

        await _unitOfWork.CommitAsync(true);

        return new SuccessResponse(Message.Contract.SuccessfullyStartContract);
    }

    public async Task<PdfCoResponse> StartProcessTerminationAsync(Guid id, Guid customerId)
    {
        var customerContractRecord = await _contractBusiness.GetForGeneratedTerminationForCustomerAsync(id, customerId);
        if (_notificationApi.HasNotification)
        {
            return default!;
        }

        var response = await _generationPdfService.GeneratePdfTerminationFromTemplateAsync(customerContractRecord);
        if (_notificationApi.HasNotification)
        {
            return default!;
        }

        await _customerContractService.StartProcessTerminationWithOutValidationAsync(customerContractRecord);
        if (_notificationApi.HasNotification)
        {
            return default!;
        }

        return response;
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
