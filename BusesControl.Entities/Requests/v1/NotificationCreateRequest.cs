using BusesControl.Entities.Enums.v1;

namespace BusesControl.Entities.Requests.v1;

public class NotificationCreateRequest
{
    public string Title { get; set; } = default!;
    public string Message { get; set; } = default!;
    public NotificationAccessLevelEnum AccessLevel { get; set; }
}
