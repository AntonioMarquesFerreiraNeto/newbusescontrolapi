using System.ComponentModel;

namespace BusesControl.Entities.Enums.v1;

public enum InvoiceStatusEnum
{
    [Description("Pendente")]
    Pending = 1,
    [Description("Paga")]
    Paid = 2,
    [Description("Atrasada")]
    OverDue = 3,
    [Description("Cancelada")]
    Canceled = 4
}
