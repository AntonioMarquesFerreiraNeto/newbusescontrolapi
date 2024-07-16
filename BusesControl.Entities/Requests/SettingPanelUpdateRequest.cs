using BusesControl.Entities.Enums;

namespace BusesControl.Entities.Requests;

public class SettingPanelUpdateRequest
{
    public decimal TerminationFee { get; set; }
    public decimal LateFeeInterestRate { get; set; }
    public bool CustomerDelinquencyEnabled { get; set; }
    public int? LimitDateTerminate { get; set; }
    public SettingPanelParentEnum Parent { get; set; }
    public bool Active { get; set; }
}
