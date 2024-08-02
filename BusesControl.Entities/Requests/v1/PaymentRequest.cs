namespace BusesControl.Entities.Requests.v1;

public class PaymentRequest
{
    public string Id { get; set; } = default!;
    public Guid ExternalReference { get; set; } = default!;
    public DateOnly? PaymentDate { get; set; } = default!;
}
