namespace BusesControl.Entities.Requests.v1;

public class TwoFaValidateCodeRequest
{
    public string Email { get; set; } = default!;
    public string Code { get; set; } = default!;
}
