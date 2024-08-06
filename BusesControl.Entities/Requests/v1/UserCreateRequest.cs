namespace BusesControl.Entities.Requests.v1;

public class UserCreateRequest
{
    public Guid EmployeeId { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Role { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
}
