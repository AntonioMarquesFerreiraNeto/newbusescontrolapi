using BusesControl.Entities.DTOs;

namespace BusesControl.Services.v1.Interfaces;

public interface IInvoiceExpenseService
{
    Task<bool> CreateInternalAsync(CreateInvoiceExpenseInDTO createInvoiceExpense);
}
