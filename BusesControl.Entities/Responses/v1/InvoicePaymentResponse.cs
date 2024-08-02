namespace BusesControl.Entities.Responses.v1;

public class InvoicePaymentResponse
{
    public string Message { get; set; } = default!;
    public PaymentPixResponse? Pix { get; set; }
}
