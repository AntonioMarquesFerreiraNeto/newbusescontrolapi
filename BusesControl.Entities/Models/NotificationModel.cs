using BusesControl.Entities.Enums;

namespace BusesControl.Entities.Models;

public class NotificationModel
{
    public Guid Id { get; set; }
    public Guid? SenderId { get; set; }
    public EmployeeModel? Sender { get; set; }
    public string Title { get; set; } = default!;
    public string Message { get; set; } = default!;
    public NotificationAccessLevelEnum AccessLevel { get; set; }
    public NotificationSenderTypeEnum SenderType { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
