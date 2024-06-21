using BusesControl.Entities.Enums;

namespace BusesControl.Entities.Requests;

public class SettingsPanelCreateRequest
{
    public decimal TerminationFee { get; set; }
    public decimal LateFeeInterestRate { get; set; }
    public bool CustomerDelinquencyEnabled { get; set; }
    public SettingsPanelParentEnum Parent { get; set; }
    public bool Active { get; set; }
}
