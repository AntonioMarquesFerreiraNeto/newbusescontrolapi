using BusesControl.Entities.Enums.v1;

namespace BusesControl.Entities.Requests.v1;

public class InvoiceExpensePaymentRequest
{
    public PaymentExpenseMethodEnum PaymentMethod { get; set; }
    public InvoiceExpensePixRequest? PixRequest { get; set; }
}
