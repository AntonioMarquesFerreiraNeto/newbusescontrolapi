namespace BusesControl.Entities.Requests.v1;

public class LoginRequest
{
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;
}
