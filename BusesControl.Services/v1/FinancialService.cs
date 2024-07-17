using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.DTOs;
using BusesControl.Entities.Enums;
using BusesControl.Entities.Models;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using BusesControl.Persistence.v1.UnitOfWork;
using BusesControl.Services.v1.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace BusesControl.Services.v1;

public class FinancialService(
    IUnitOfWork _unitOfWork,
    INotificationApi _notificationApi,
    IInvoiceService _invoiceService,
    IFinancialRepository _financialRepository
) : IFinancialService
{
    public async Task<bool> CreateForContractAsync(ContractModel contractRecord)
    {
        var title = $"Referente ao contrato Nº {contractRecord.Reference}";
        var description = $"Financeiro referente ao contrato de locação de frota Nº {contractRecord.Reference}, iniciado em {contractRecord.StartDate!.Value.ToString("dd 'de' MMMM 'de' yyyy", new CultureInfo("pt-BR"))}.";
        var priceFinancial = Math.Round(contractRecord.TotalPrice / contractRecord.CustomersCount, 2);

        foreach (var customerContract in contractRecord.CustomersContract)
        {
            var record = new FinancialModel
            {
                ContractId = contractRecord.Id,
                CustomerId = customerContract.CustomerId,
                Title = title,
                Description = description,
                TotalPrice = priceFinancial,
                Price = priceFinancial,
                StartDate = contractRecord.StartDate.Value,
                TerminateDate = contractRecord.TerminateDate,
                InstallmentsCount = contractRecord.InstallmentsCount!.Value,
                Type = FinancialTypeEnum.Expense,
                PaymentType = contractRecord.PaymentType
            };
            await _financialRepository.CreateAsync(record);
            await _unitOfWork.CommitAsync();

            for (int index = 1; index <= record.InstallmentsCount; index++)
            {
                var createInvoice = new CreateInvoiceDTO
                { 
                    FinancialId = record.Id,
                    ExternalId = customerContract.Customer!.ExternalId,
                    DueDate = (index != 1) ? record.StartDate.AddMonths(index - 1) : record.StartDate,
                    Price = Math.Round(record.TotalPrice / record.InstallmentsCount, 2),
                    Index = index,
                    Reference = contractRecord.Reference,
                    PaymentType = record.PaymentType,
                    FinancialType = FinancialTypeEnum.Revenue,
                    IsContract = true
                };

                await _invoiceService.CreateForFinancialAsync(createInvoice);
                if (_notificationApi.HasNotification)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public async Task<bool> ToggleActiveForTerminationAsync(Guid contractId, Guid customerId)
    {
        var financialRecord = await _financialRepository.GetByContractAndCustomerWithInvoicesAsync(contractId, customerId);
        if (financialRecord is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Financial.NotFound
            );
            return default!;
        }

        financialRecord.Active = false;
        _financialRepository.Update(financialRecord);
        await _unitOfWork.CommitAsync();

        var invoices = financialRecord.Invoices.Where(x => x.Status == InvoiceStatusEnum.Pending);

        foreach (var invoice in invoices)
        {
            await _invoiceService.CancelForFinancialAsync(invoice);
            if (_notificationApi.HasNotification)
            {
                return false;
            }
        }

        return true;
    }
}
