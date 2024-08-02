using BusesControl.Entities.Enums.v1;

namespace BusesControl.Entities.Responses.v1;

public class SettingPanelResponse
{
    public Guid Id { get; set; }
    public decimal TerminationFee { get; set; }
    public decimal LateFeeInterestRate { get; set; }
    public bool CustomerDelinquencyEnabled { get; set; }
    public int? LimitDateTermination { get; set; }
    public SettingPanelParentEnum Parent { get; set; }
    public bool Active { get; set; }
    public Guid RequesterId { get; set; }
    public EmployeeResponse Requester { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
