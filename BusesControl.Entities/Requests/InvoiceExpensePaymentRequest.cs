using BusesControl.Entities.Enums;

namespace BusesControl.Entities.Requests;

public class InvoiceExpensePaymentRequest
{
    public PaymentExpenseMethodEnum PaymentMethod { get; set; }
    public InvoiceExpensePixRequest? PixRequest { get; set; }
}
