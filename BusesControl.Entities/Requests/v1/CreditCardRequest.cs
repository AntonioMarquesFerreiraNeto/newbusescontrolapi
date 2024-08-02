namespace BusesControl.Entities.Requests.v1;

public class CreditCardRequest
{
    public string HolderName { get; set; } = default!;
    public string HolderCpfCnpj { get; set; } = default!;
    public string HolderEmail { get; set; } = default!;
    public string HolderMobilePhone { get; set; } = default!;
    public string HolderPostalCode { get; set; } = default!;
    public string HolderAddressNumber { get; set; } = default!;
    public string Number { get; set; } = default!;
    public string ExpiryMonth { get; set; } = default!;
    public string ExpiryYear { get; set; } = default!;
    public string SecurityCode { get; set; } = default!;
}
