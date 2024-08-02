namespace BusesControl.Entities.Requests.v1;

public class UserResetPasswordStepResetTokenRequest
{
    public string Code { get; set; } = default!;
}
