using BusesControl.Entities.DTOs;
using BusesControl.Entities.Requests;
using BusesControl.Entities.Responses;

namespace BusesControl.Services.v1.Interfaces;

public interface IInvoiceService
{
    Task<bool> CreateForFinancialAsync(CreateInvoiceDTO createInvoice);
    Task<InvoicePaymentResponse> PaymentAsync(Guid id, InvoicePaymentRequest request);
}
