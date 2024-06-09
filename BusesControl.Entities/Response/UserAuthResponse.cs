namespace BusesControl.Entities.Response;

public class UserAuthResponse
{
    public Guid Id { get; set; }
    public string Role { get; set; } = default!;

    public UserAuthResponse(Guid id, string role)
    {
        Id = id;
        Role = role;
    }
}
