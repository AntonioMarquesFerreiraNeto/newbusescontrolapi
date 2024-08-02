using BusesControl.Entities.Enums.v1;

namespace BusesControl.Entities.Requests.v1;

public class SettingPanelUpdateRequest
{
    public decimal TerminationFee { get; set; }
    public decimal LateFeeInterestRate { get; set; }
    public bool CustomerDelinquencyEnabled { get; set; }
    public int? LimitDateTerminate { get; set; }
    public SettingPanelParentEnum Parent { get; set; }
    public bool Active { get; set; }
}
