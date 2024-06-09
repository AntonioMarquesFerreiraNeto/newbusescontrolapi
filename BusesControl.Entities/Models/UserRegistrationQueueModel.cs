﻿using BusesControl.Entities.Enums;

namespace BusesControl.Entities.Models;

public class UserRegistrationQueueModel
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public EmployeeModel Employee { get; set; } = default!;
    public Guid RequestId { get; set; }
    public Guid? ApprovedId { get; set; }
    public Guid? UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public UserRegistrationQueueStatusEnum Status { get; set; } = UserRegistrationQueueStatusEnum.Started;
}
