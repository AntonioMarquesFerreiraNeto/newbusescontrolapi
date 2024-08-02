using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Responses.v1;

namespace BusesControl.Business.v1.Interfaces;

public interface IInvoiceBusiness
{
    Task<InvoiceModel> GetForPaymentAsync(Guid id);
    Task<InvoiceModel> GetForPaymentPixAsync(Guid id, string externalId);
    bool ValidateLoggedUserForJustCountPayment(UserAuthResponse loggedUser);
}
