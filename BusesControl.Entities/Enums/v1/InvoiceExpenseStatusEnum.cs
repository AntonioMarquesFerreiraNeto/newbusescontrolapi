using System.ComponentModel;

namespace BusesControl.Entities.Enums.v1;

public enum InvoiceExpenseStatusEnum
{
    [Description("Pendente")]
    Pending = 1,
    [Description("Paga")]
    Paid = 2,
    [Description("Cancelada")]
    Canceled = 3
}
