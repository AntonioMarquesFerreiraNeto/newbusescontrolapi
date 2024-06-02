namespace BusesControl.Commons.Notification.Interfaces;

public interface INotificationApi
{
    bool HasNotification { get; }
    int? StatusCodes { get; }
    string Title { get; }
    string Details { get; }
    void SetNotification(string title, string details, int statusCode);
    object GetDetails();
}
