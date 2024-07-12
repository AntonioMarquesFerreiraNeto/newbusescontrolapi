namespace BusesControl.Entities.Responses;

public class PaymentPixResponse
{
    public string EncodedImage { get; set; } = default!;
    public string Payload { get; set; } = default!;
    public string ExpirationDate { get; set; } = default!;
}
