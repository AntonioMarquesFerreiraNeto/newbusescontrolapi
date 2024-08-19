using BusesControl.Entities.Enums.v1;

namespace BusesControl.Entities.Responses.v1;

public class UserResponse
{
    public Guid Id { get; set; }
    public string NickName { get; set; } = default!;
    public string Role { get; set; } = default!;
    public string Email { get; set; } = default!;
    public Guid EmployeeId { get; set; }
    public EmployeeResponse Employee { get; set; } = default!;
    public UserStatusEnum Status { get; set; }
}
