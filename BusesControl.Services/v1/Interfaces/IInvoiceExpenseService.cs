using BusesControl.Entities.DTOs;
using BusesControl.Entities.Models;
using BusesControl.Entities.Requests;
using BusesControl.Entities.Response;

namespace BusesControl.Services.v1.Interfaces;

public interface IInvoiceExpenseService
{
    Task<bool> CreateInternalAsync(CreateInvoiceExpenseInDTO createInvoiceExpense);
    Task<bool> CancelInternalAsync(IEnumerable<InvoiceExpenseModel> records);
    Task<SuccessResponse> PaymentAsync(Guid id, InvoiceExpensePaymentRequest request);
}
