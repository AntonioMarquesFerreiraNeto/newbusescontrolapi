namespace BusesControl.Entities.Requests;

public class UserUpdateRequest
{
    public string NewEmail { get; set; } = default!;
    public string NewPhoneNumber { get; set; } = default!;
    public string OldEmail { get; set; } = default!;
}
