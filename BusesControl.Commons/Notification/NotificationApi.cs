using BusesControl.Commons.Notification.Interfaces;

namespace BusesControl.Filters.Notification;

public class NotificationApi : INotificationApi
{
    public bool HasNotification { get; private set; }
    public int? StatusCodes { get; private set; }
    public string Title { get; set; } = default!;
    public string Details { get; set; } = default!;

    public void SetNotification(string title, string details, int statusCode)
    {
        Title = title;
        Details = details;
        StatusCodes = statusCode;
        HasNotification = true;
    }

    public void Reset()
    {
        Title = default!;
        Details = default!;
        StatusCodes = null;
        HasNotification = false;
    }
}
