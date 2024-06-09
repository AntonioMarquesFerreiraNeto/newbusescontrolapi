namespace BusesControl.Entities.Request;

public class UserRegistrationStepPasswordRequest
{
    public Guid UserId { get; set; } = default!;
    public string NewPassword { get; set; } = default!;
    public string ConfirmPassword { get; set; } = default!;
    public string ResetToken { get; set; } = default!;
}
