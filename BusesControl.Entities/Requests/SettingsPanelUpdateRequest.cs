using BusesControl.Entities.Enums;

namespace BusesControl.Entities.Requests;

public class SettingsPanelUpdateRequest
{
    public decimal TerminationFee { get; set; }
    public decimal LateFeeInterestRate { get; set; }
    public bool CustomerDelinquencyEnabled { get; set; }
    public int? LimitDateTermination { get; set; }
    public SettingsPanelParentEnum Parent { get; set; }
    public bool Active { get; set; }
}
