using BusesControl.Entities.Enums;

namespace BusesControl.Entities.Requests;

public class InvoicePaymentRequest
{
    public PaymentMethodEnum PaymentMethod { get; set; }
    public CreditCardRequest? CreditCard { get; set; }
}
