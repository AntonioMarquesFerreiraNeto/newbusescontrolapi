using BusesControl.Entities.Enums;

namespace BusesControl.Entities.Responses;

public class SettingsPanelResponse
{
    public decimal TerminationFee { get; set; }
    public decimal LateFeeInterestRate { get; set; }
    public bool CustomerDelinquencyEnabled { get; set; }
    public SettingsPanelParentEnum Parent { get; set; }
    public bool Active { get; set; }
    public Guid RequesterId { get; set; }
    public UserResponse Requester { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
