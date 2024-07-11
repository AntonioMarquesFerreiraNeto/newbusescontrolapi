using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.DTOs;
using BusesControl.Entities.Enums;
using BusesControl.Entities.Models;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using BusesControl.Persistence.v1.UnitOfWork;
using BusesControl.Services.v1.Interfaces;
using System.Globalization;

namespace BusesControl.Services.v1;

public class FinancialService(
    IUnitOfWork _unitOfWork,
    INotificationApi _notificationApi,
    IFinancialRepository _financialRepository,
    IInvoiceService _invoiceService
) : IFinancialService
{
    public async Task<bool> CreateForContractAsync(ContractModel contractRecord)
    {
        var title = $"Referente ao contrato Nº {contractRecord.Reference}";
        var description = $"Financeiro referente ao contrato de locação de frota Nº {contractRecord.Reference}, iniciado em {contractRecord.StartDate!.Value.ToString("dd 'de' MMMM 'de' yyyy", new CultureInfo("pt-BR"))}.";
        var priceFinancial = Math.Round(contractRecord.TotalPrice / contractRecord.CustomersCount, 2);

        foreach (var customerContract in contractRecord.CustomersContract)
        {
            var financialRecord = new FinancialModel
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
            await _financialRepository.CreateAsync(financialRecord);
            await _unitOfWork.CommitAsync();

            for (int index = 1; index <= financialRecord.InstallmentsCount; index++)
            {
                var createInvoice = new CreateInvoiceDTO
                { 
                    FinancialId = financialRecord.Id,
                    ExternalId = customerContract.Customer!.ExternalId,
                    DueDate = (index != 1) ? financialRecord.StartDate.AddMonths(index - 1) : financialRecord.StartDate,
                    Price = Math.Round(financialRecord.TotalPrice / financialRecord.InstallmentsCount, 2),
                    Index = index,
                    ContractReference = contractRecord.Reference,
                    PaymentType = financialRecord.PaymentType
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
}
