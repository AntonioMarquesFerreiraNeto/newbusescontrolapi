using System.ComponentModel;

namespace BusesControl.Entities.Enums.v1;

public enum FinancialTypeEnum
{
    [Description("Receita")]
    Revenue = 1,
    [Description("Despesa")]
    Expense = 2,
}
