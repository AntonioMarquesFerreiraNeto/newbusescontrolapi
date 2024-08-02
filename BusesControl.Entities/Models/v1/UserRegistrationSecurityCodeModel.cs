namespace BusesControl.Entities.Models.v1;

public class UserRegistrationSecurityCodeModel
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public UserModel User { get; set; } = default!;
    public string Code { get; set; } = default!;
    public DateTime Expires { get; set; }
}
