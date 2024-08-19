using BusesControl.Entities.Enums.v1;

namespace BusesControl.Entities.Requests.v1;

public class UserToggleActiveRequest
{
    public UserStatusEnum Status { get; set; }
}
