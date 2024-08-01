using System.ComponentModel;

namespace BusesControl.Entities.Enums;

public enum InvoiceExpenseStatusEnum
{
    [Description("Pendente")]
    Pending = 1,
    [Description("Paga")]
    Paid = 2,
    [Description("Cancelada")]
    Canceled = 3
}
