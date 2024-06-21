namespace BusesControl.Entities.Response;

public class UserResetPasswordStepResetTokenResponse
{
    public Guid UserId { get; set; } = default!;
    public string Token { get; set; } = default!;

    public UserResetPasswordStepResetTokenResponse(Guid userId, string token)
    {
        UserId = userId;
        Token = token;
    }
}
