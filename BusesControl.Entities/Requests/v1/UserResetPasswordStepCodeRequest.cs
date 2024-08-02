namespace BusesControl.Entities.Requests.v1;

public class UserResetPasswordStepCodeRequest
{
    public string Email { get; set; } = default!;
    public string Cpf { get; set; } = default!;
    public DateOnly BirthDate { get; set; } = default!;
}
