using System.ComponentModel;

namespace BusesControl.Entities.Enums;

public enum FinancialTypeEnum
{
    [Description("Receita")]
    Revenue = 1,
    [Description("Despesa")]
    Expense = 2,
}
