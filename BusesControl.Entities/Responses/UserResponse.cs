namespace BusesControl.Entities.Responses;

public class UserResponse
{
    public string NickName { get; set; } = default!;
    public string Role { get; set; } = default!;
    public string Email { get; set; } = default!;
    public Guid EmployeeId { get; set; }
}
