namespace BusesControl.Entities.Requests;

public class PaymentPixRequest
{
    public string Event { get; set; } = default!;
    public PaymentRequest Payment { get; set; } = default!;
}
