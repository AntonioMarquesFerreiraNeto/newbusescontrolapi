using BusesControl.Entities.Models;
using BusesControl.Entities.Response;

namespace BusesControl.Business.v1.Interfaces;

public interface IInvoiceExpenseBusiness
{
    Task<InvoiceExpenseModel> GetForPaymentAsync(Guid id);
    Task<bool> ValidateBalanceInAssasAsync(decimal pricePayment);
    bool ValidateLoggedUserForJustCountPayment(UserAuthResponse loggedUser);
}
