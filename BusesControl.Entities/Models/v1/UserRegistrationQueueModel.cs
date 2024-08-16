using BusesControl.Entities.Enums.v1;

namespace BusesControl.Entities.Models.v1;

public class UserRegistrationQueueModel
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public EmployeeModel Employee { get; set; } = default!;
    public Guid RequesterId { get; set; }
    public EmployeeModel Requester { get; set; } = default!;
    public Guid? ApprovedId { get; set; }
    public EmployeeModel? Approved { get; set; }
    public Guid? UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public UserRegistrationQueueStatusEnum Status { get; set; } = UserRegistrationQueueStatusEnum.Started;
}
