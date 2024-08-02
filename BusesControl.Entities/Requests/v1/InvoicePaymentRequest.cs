using BusesControl.Entities.Enums.v1;

namespace BusesControl.Entities.Requests.v1;

public class InvoicePaymentRequest
{
    public PaymentMethodEnum PaymentMethod { get; set; }
    public CreditCardRequest? CreditCard { get; set; }
}
