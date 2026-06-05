using BusesControl.Entities.DTOs;
using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Requests.v1;
using BusesControl.Entities.Responses.v1;

namespace BusesControl.Services.v1.Interfaces
{
    public interface IAsaasService
    {
        Task<string> CreateCustomerAsync(CustomerModel customer);
        Task<bool> UpdateCustomerAsync(string externalId, CustomerUpdateRequest customer);
        Task<string> CreatePaymentAsync(Guid id, string externalId, string descriptionInvoice, CreateInvoiceDTO createInvoice);
        Task<bool> UpdatePaymentAsync(InvoiceModel updateInvoice, decimal interest = 0);
        Task<bool> RemovePaymentAsync(string externalId);
        Task<InvoicePayWithCardInAssasDTO> CreditCardPaymentAsync(InvoiceModel record, InvoicePaymentRequest request);
        Task<PaymentPixResponse> PixPaymentAsync(InvoiceModel record);
        Task<AutomatedPaymentResponse> AutomatedPaymentAsync(string? externalId, Guid creditCardToken);
    }
}
