using System.ComponentModel;

namespace BusesControl.Entities.Enums;

public enum FinancialTypeEnum
{
    [Description("Receita")]
    Expense = 1,
    [Description("Despesa")]
    Revenue = 2,
    [Description("Rescisão")]
    Termination = 3
}
