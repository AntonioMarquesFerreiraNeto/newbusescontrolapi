namespace BusesControl.Entities.Request;

public class UserRegistrationStepCodeRequest
{
    public string Email { get; set; } = default!;
    public string Cpf { get; set; } = default!;
    public DateOnly BirthDate { get; set; } = default!;
}
