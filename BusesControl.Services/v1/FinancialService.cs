using AutoMapper;
using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.DTOs;
using BusesControl.Entities.Enums.v1;
using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Requests.v1;
using BusesControl.Entities.Responses.v1;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using BusesControl.Persistence.UnitOfWork;
using BusesControl.Services.v1.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace BusesControl.Services.v1;

public class FinancialService(
    IMapper _mapper,
    IUnitOfWork _unitOfWork,
    INotificationContext _notificationContext,
    IInvoiceService _invoiceService,
    IInvoiceExpenseService _invoiceExpenseService,
    IFinancialBusiness _financialBusiness,
    ICustomerBusiness _customerBusiness,
    ISupplierBusiness _supplierBusiness,
    ISettingPanelBusiness _settingPanelBusiness,
    IFinancialRepository _financialRepository
) : IFinancialService
{
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
            existsReference = await _financialRepository.ExistsByReferenceAsync(reference);
        }

        return reference;
    }

    private static DateOnly GetStartDateFormated()
    {
        var dateNow = DateTime.UtcNow;
        return (dateNow.Day > 28) ? new DateOnly(dateNow.Year, dateNow.AddMonths(1).Month, 1) : DateOnly.FromDateTime(dateNow);
    }

    private static int CalculateMonths(DateOnly startDate, DateOnly endDate)
    {
        int startYear = startDate.Year;
        int startMonth = startDate.Month;

        int endYear = endDate.Year;
        int endMonth = endDate.Month;

        int monthDifference = (endYear - startYear) * 12 + (endMonth - startMonth);

        return monthDifference;
    }

    public async Task<IEnumerable<FinancialModel>> FindBySearchAsync(PaginationRequest request)
    {
        var records = await _financialRepository.FindBySearchAsync(request.Page, request.PageSize, request.Search);
        return records;
    }

    public async Task<FinancialResponse> GetByIdAsync(Guid id)
    {
        var record = await _financialRepository.GetByIdWithIncludesAsync(id);
        if (record is null)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Financial.NotFound
            );
            return default!;
        }

        return _mapper.Map<FinancialResponse>(record);
    }

    public async Task<bool> CreateRevenueAsync(FinancialRevenueCreateRequest request)
    {
        var customerRecord = await _customerBusiness.GetWithValidateActiveAsync(request.CustomerId);
        if (_notificationContext.HasNotification)
        {
            return false;
        }

        var settingPanelRecord = await _settingPanelBusiness.GetForCreateFinancialRevenueAsync(request.SettingPanelId);
        if (_notificationContext.HasNotification)
        {
            return false;
        }

        _financialBusiness.ValidateTerminationDate(settingPanelRecord, request.TerminateDate);
        if (_notificationContext.HasNotification)
        {
            return false;
        }

        _unitOfWork.BeginTransaction();

        var installments = request.PaymentType == PaymentTypeEnum.Multiple ? CalculateMonths(DateOnly.FromDateTime(DateTime.UtcNow), request.TerminateDate) : 1;

        var record = new FinancialModel
        {
            Reference = await GenerateReferenceUniqueAsync(),
            SettingPanelId = settingPanelRecord.Id,
            CustomerId = request.CustomerId,
            Title = request.Title,
            Description = request.Description,
            TotalPrice = request.TotalPrice,
            Price = request.TotalPrice,
            Type = FinancialTypeEnum.Revenue,
            PaymentType = request.PaymentType,
            StartDate = GetStartDateFormated(),
            TerminateDate = request.TerminateDate,
            InstallmentsCount = installments
        };
        await _financialRepository.AddAsync(record);
        await _unitOfWork.CommitAsync();

        for (var index = 1; index <= installments; index++)
        {
            var createInvoice = new CreateInvoiceDTO
            {
                Index = index,
                CustomerExternalId = customerRecord.ExternalId,
                Reference = record.Reference,
                FinancialId = record.Id,
                Price = Math.Round(record.TotalPrice / record.InstallmentsCount, 2),
                FinancialType = record.Type,
                PaymentType = record.PaymentType,
                DueDate = (index != 1) ? record.StartDate.AddMonths(index - 1) : record.StartDate,
                IsContract = false
            };

            await _invoiceService.CreateInternalAsync(createInvoice);
            if (_notificationContext.HasNotification)
            {
                return false;
            }
        }

        await _unitOfWork.CommitAsync(true);

        return true;
    }

    public async Task<bool> CreateExpenseAsync(FinancialExpenseCreateRequest request)
    {
        var supplierRecord = await _supplierBusiness.GetWithValidateActiveAsync(request.SupplierId);
        if (_notificationContext.HasNotification)
        {
            return false;
        }

        var settingPanelRecord = await _settingPanelBusiness.GetForCreateFinancialExpenseAsync(request.SettingPanelId);
        if (_notificationContext.HasNotification)
        {
            return false;
        }

        _financialBusiness.ValidateTerminationDate(settingPanelRecord, request.TerminateDate);
        if (_notificationContext.HasNotification)
        {
            return false;
        }

        _unitOfWork.BeginTransaction();

        var installments = request.PaymentType == PaymentTypeEnum.Multiple ? CalculateMonths(DateOnly.FromDateTime(DateTime.UtcNow), request.TerminateDate) : 1;

        var record = new FinancialModel
        {
            Reference = await GenerateReferenceUniqueAsync(),
            SettingPanelId = request.SettingPanelId,
            SupplierId = supplierRecord.Id,
            Title = request.Title,
            TotalPrice = request.TotalPrice,
            Price = request.TotalPrice,
            Description = request.Description,
            StartDate = GetStartDateFormated(),
            TerminateDate = request.TerminateDate,
            InstallmentsCount = installments,
            PaymentType = request.PaymentType,
            Type = FinancialTypeEnum.Expense
        };
        await _financialRepository.AddAsync(record);
        await _unitOfWork.CommitAsync();

        for (var index = 1; index <= installments; index++)
        {
            var createInvoiceExpense = new CreateInvoiceExpenseInDTO
            {
                Index = index,
                FinancialId = record.Id,
                FinancialReference = record.Reference,
                Price = Math.Round(record.TotalPrice / installments, 2),
                DueDate = index != 1 ? record.StartDate.AddMonths(index - 1) : record.StartDate,
                IsSingle = record.PaymentType == PaymentTypeEnum.Single
            };
            
            await _invoiceExpenseService.CreateInternalAsync(createInvoiceExpense);
        }

        await _unitOfWork.CommitAsync(true);

        return true;
    }

    public async Task<bool> InactiveRevenueAsync(Guid id)
    {
        var record = await _financialBusiness.GetForInactiveRevenueAsync(id);
        if (_notificationContext.HasNotification)
        {
            return false;
        }

        _unitOfWork.BeginTransaction();

        var invoices = record.Invoices.Where(x => x.Status == InvoiceStatusEnum.Pending);

        foreach (var invoice in invoices)
        {
            await _invoiceService.CancelInternalAsync(invoice);
            if (_notificationContext.HasNotification)
            {
                return false;
            }
        }

        record.Active = false;
        record.UpdatedAt = DateTime.UtcNow;
        _financialRepository.Update(record);
        await _unitOfWork.CommitAsync();

        await _unitOfWork.CommitAsync(true);

        return true;
    }

    public async Task<bool> InactiveExpenseAsync(Guid id)
    {
        var record = await _financialBusiness.GetForInactiveExpenseAsync(id);
        if (_notificationContext.HasNotification)
        {
            return false;
        }

        _unitOfWork.BeginTransaction();

        await _invoiceExpenseService.CancelInternalAsync(record.InvoiceExpenses);
        if (_notificationContext.HasNotification)
        {
            return false;
        }

        record.UpdatedAt = DateTime.UtcNow;
        record.Active = false;
        _financialRepository.Update(record);
        await _unitOfWork.CommitAsync();

        await _unitOfWork.CommitAsync(true);

        return true;
    }

    public async Task<bool> UpdateDetailsAsync(Guid id, FinancialUpdateDetailsRequest request)
    {
        var record = await _financialBusiness.GetForUpdateDetailsAsync(id);
        if (_notificationContext.HasNotification)
        {
            return false;
        }

        record.UpdatedAt = DateTime.UtcNow;
        record.Title = request.Title;
        record.Description = request.Description;
        _financialRepository.Update(record);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> CreateInternalAsync(ContractModel contractRecord)
    {
        var title = $"Referente ao contrato {contractRecord.Reference}";
        var description = $"Financeiro referente ao contrato de locação de frota {contractRecord.Reference}, iniciado em {contractRecord.StartDate!.Value.ToString("dd 'de' MMMM 'de' yyyy", new CultureInfo("pt-BR"))}.";
        var priceFinancial = Math.Round(contractRecord.TotalPrice / contractRecord.CustomersCount, 2);

        foreach (var customerContract in contractRecord.CustomersContract)
        {
            var record = new FinancialModel
            {
                Reference = await GenerateReferenceUniqueAsync(),
                SettingPanelId = contractRecord.SettingPanelId,
                ContractId = contractRecord.Id,
                CustomerId = customerContract.CustomerId,
                Title = title,
                Description = description,
                TotalPrice = priceFinancial,
                Price = priceFinancial,
                StartDate = GetStartDateFormated(),
                TerminateDate = contractRecord.TerminateDate,
                InstallmentsCount = contractRecord.InstallmentsCount!.Value,
                Type = FinancialTypeEnum.Revenue,
                PaymentType = contractRecord.PaymentType
            };
            await _financialRepository.AddAsync(record);
            await _unitOfWork.CommitAsync();

            for (int index = 1; index <= record.InstallmentsCount; index++)
            {
                var createInvoice = new CreateInvoiceDTO
                { 
                    Index = index,
                    Reference = contractRecord.Reference,
                    FinancialId = record.Id,
                    CustomerExternalId = customerContract.Customer!.ExternalId,
                    DueDate = (index != 1) ? record.StartDate.AddMonths(index - 1) : record.StartDate,
                    Price = Math.Round(record.TotalPrice / record.InstallmentsCount, 2),
                    PaymentType = record.PaymentType,
                    FinancialType = FinancialTypeEnum.Revenue,
                    IsContract = true
                };

                await _invoiceService.CreateInternalAsync(createInvoice);
                if (_notificationContext.HasNotification)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public async Task<bool> InactiveInternalAsync(Guid contractId, Guid customerId)
    {
        var record = await _financialRepository.GetByContractAndCustomerWithInvoicesAsync(contractId, customerId);
        if (record is null)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Financial.NotFound
            );
            return default!;
        }

        var invoices = record.Invoices.Where(x => x.Status == InvoiceStatusEnum.Pending);

        foreach (var invoice in invoices)
        {
            await _invoiceService.CancelInternalAsync(invoice);
            if (_notificationContext.HasNotification)
            {
                return false;
            }
        }

        record.Active = false;
        record.UpdatedAt = DateTime.UtcNow;
        _financialRepository.Update(record);
        await _unitOfWork.CommitAsync();

        return true;
    }
}
