namespace BusesControl.Entities.Response;

public class UserRegistrationStepTokenResponse
{
    public Guid UserId { get; set; } = default!;
    public string Token { get; set; } = default!;

    public UserRegistrationStepTokenResponse(Guid userId, string token)
    {
        UserId = userId;
        Token = token;
    }
}
