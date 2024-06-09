using BusesControl.Entities.Enums;

namespace BusesControl.Entities.Models;

public class UserRegistrationQueueModel
{
    public Guid Id { get; set; }
    public Guid RequestId { get; set; }
    public Guid EmployeeId { get; set; }
    public DateTime RequestDate { get; set; } = DateTime.UtcNow;
    public UserRegistrationQueueStatusEnum Status { get; set; } = UserRegistrationQueueStatusEnum.Pending;
}
