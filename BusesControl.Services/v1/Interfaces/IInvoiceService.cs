using BusesControl.Entities.DTOs;
using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Requests.v1;
using BusesControl.Entities.Responses.v1;

namespace BusesControl.Services.v1.Interfaces;

public interface IInvoiceService
{
    Task<bool> CreateInternalAsync(CreateInvoiceDTO createInvoice);
    Task<bool> CancelInternalAsync(InvoiceModel record);
    Task<InvoicePaymentResponse> PaymentAsync(Guid id, InvoicePaymentRequest request);
    Task<AutomatedPaymentResponse> AutomatedPaymentAsync(InvoiceModel record, Guid creditCardToken);
    Task<(bool success, string? errorMessage)> ChangeOverDueInternalAsync(InvoiceModel record);
}
