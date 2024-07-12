using BusesControl.Entities.DTOs;
using BusesControl.Entities.Models;
using BusesControl.Entities.Requests;
using BusesControl.Entities.Responses;

namespace BusesControl.Services.v1.Interfaces;

public interface IInvoiceService
{
    Task<bool> CreateForFinancialAsync(CreateInvoiceDTO createInvoice);
    Task<InvoicePaymentResponse> PaymentAsync(Guid id, InvoicePaymentRequest request);
    Task<AutomatedPaymentResponse> AutomatedPaymentAsync(InvoiceModel record, Guid creditCardToken);
    Task<(bool success, string? errorMessage)> ChangeOverDueForSystemAsync(InvoiceModel record);
}
