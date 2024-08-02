using BusesControl.Entities.DTOs;
using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Requests.v1;
using BusesControl.Entities.Responses.v1;

namespace BusesControl.Services.v1.Interfaces;

public interface IInvoiceExpenseService
{
    Task<bool> CreateInternalAsync(CreateInvoiceExpenseInDTO createInvoiceExpense);
    Task<bool> CancelInternalAsync(IEnumerable<InvoiceExpenseModel> records);
    Task<SuccessResponse> PaymentAsync(Guid id, InvoiceExpensePaymentRequest request);
}
