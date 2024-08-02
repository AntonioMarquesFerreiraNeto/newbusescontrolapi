using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Responses.v1;

namespace BusesControl.Business.v1.Interfaces;

public interface IInvoiceExpenseBusiness
{
    Task<InvoiceExpenseModel> GetForPaymentAsync(Guid id);
    Task<bool> ValidateBalanceInAssasAsync(decimal pricePayment);
    bool ValidateLoggedUserForJustCountPayment(UserAuthResponse loggedUser);
}
