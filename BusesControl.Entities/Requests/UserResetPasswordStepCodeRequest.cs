namespace BusesControl.Entities.Request;

public class UserResetPasswordStepCodeRequest
{
    public string Email { get; set; } = default!;
    public string Cpf { get; set; } = default!;
    public DateOnly BirthDate { get; set; } = default!;
}
