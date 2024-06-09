namespace BusesControl.Entities.Request;

public class UserCreateRequest
{
    public Guid EmployeeId { get; set; }
    public string Email { get; set; } = default!;
    public string Role { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string Password { get; set; } = default!;
}
