using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.DTOs;
using BusesControl.Entities.Models;
using BusesControl.Persistence.v1.Repositories;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using BusesControl.Persistence.v1.UnitOfWork;
using BusesControl.Services.v1.Interfaces;

namespace BusesControl.Services.v1;

public class InvoiceExpenseService(
    IUnitOfWork _unitOfWork,
    IInvoiceExpenseRepository _invoiceExpenseRepository
) : IInvoiceExpenseService
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
            existsReference = await _invoiceExpenseRepository.ExistsByReferenceAsync(reference);
        }

        return reference;
    }

    public async Task<bool> CreateInternalAsync(CreateInvoiceExpenseInDTO createInvoiceExpense)
    {
        var record = new InvoiceExpenseModel
        {
            Reference = await GenerateReferenceUniqueAsync(),
            FinancialId = createInvoiceExpense.FinancialId,
            Title = "Fatura referente a despesa",
            Description = $"{createInvoiceExpense.Index}º fatura referente a despesa do financeiro {createInvoiceExpense.FinancialReference}",
            Price = createInvoiceExpense.Price,
            TotalPrice = createInvoiceExpense.Price,
            DueDate = createInvoiceExpense.DueDate
        };
        await _invoiceExpenseRepository.CreateAsync(record);
        await _unitOfWork.CommitAsync();

        return true;
    }
}
