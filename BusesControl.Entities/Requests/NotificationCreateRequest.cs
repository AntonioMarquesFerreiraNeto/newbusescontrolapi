using BusesControl.Entities.Enums;

namespace BusesControl.Entities.Requests;

public class NotificationCreateRequest
{
    public string Title { get; set; } = default!;
    public string Message { get; set; } = default!;
    public NotificationAccessLevelEnum AccessLevel { get; set; }
}
