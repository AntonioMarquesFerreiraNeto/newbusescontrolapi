using BusesControl.Entities.Enums;

namespace BusesControl.Entities.Requests;

public class SettingPanelCreateRequest
{
    public decimal TerminationFee { get; set; }
    public decimal LateFeeInterestRate { get; set; }
    public bool CustomerDelinquencyEnabled { get; set; }
    public int? LimitDateTermination { get; set; }
    public SettingPanelParentEnum Parent { get; set; }
    public bool Active { get; set; }
}
