namespace BusesControl.Entities.Requests.v1;

public class TwoFaValidateTokenRequest
{
    public string Email { get; set; } = default!;
    public string? Token { get; set; } = default!;
    public string? IpLocation { get; set; }
}
