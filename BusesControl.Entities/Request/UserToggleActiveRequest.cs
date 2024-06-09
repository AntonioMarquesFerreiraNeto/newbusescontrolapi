using BusesControl.Entities.Enums;

namespace BusesControl.Entities.Request;

public class UserToggleActiveRequest
{
    public UserStatusEnum Status { get; set; }

    public UserToggleActiveRequest() {}

    public UserToggleActiveRequest(UserStatusEnum status)
    {
        Status = status;
    }
}
