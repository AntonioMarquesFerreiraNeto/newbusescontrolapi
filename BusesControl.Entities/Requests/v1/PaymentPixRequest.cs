namespace BusesControl.Entities.Requests.v1;

public class PaymentPixRequest
{
    public string Event { get; set; } = default!;
    public PaymentRequest Payment { get; set; } = default!;
}
