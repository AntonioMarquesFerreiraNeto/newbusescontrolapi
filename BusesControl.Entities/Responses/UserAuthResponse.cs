namespace BusesControl.Entities.Response;

public class UserAuthResponse
{
    public Guid Id { get; set; }
    public string Role { get; set; } = default!;
    public Guid? EmployeeId { get; set; }

    public UserAuthResponse(Guid id, string role, Guid? employeeId)
    {
        Id = id;
        Role = role;
        EmployeeId = employeeId;
    }
}
