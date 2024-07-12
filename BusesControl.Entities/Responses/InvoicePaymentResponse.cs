namespace BusesControl.Entities.Responses;

public class InvoicePaymentResponse
{
    public string Message { get; set; } = default!;
    public PaymentPixResponse? Pix { get; set; }
}
