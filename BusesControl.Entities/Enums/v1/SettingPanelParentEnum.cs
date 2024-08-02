using System.ComponentModel;

namespace BusesControl.Entities.Enums.v1;

public enum SettingPanelParentEnum
{
    [Description("Contrato")]
    Contract = 1,
    [Description("Receita")]
    Revenue = 2,
    [Description("Despesa")]
    Expense = 3
}
