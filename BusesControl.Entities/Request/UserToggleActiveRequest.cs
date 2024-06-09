using BusesControl.Entities.Enums;

namespace BusesControl.Entities.Request;

public class UserToggleActiveRequest
{
    public UserStatusEnum Status { get; set; }
}
