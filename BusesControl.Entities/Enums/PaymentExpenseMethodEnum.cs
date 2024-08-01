using System.ComponentModel;

namespace BusesControl.Entities.Enums;

public enum PaymentExpenseMethodEnum
{
    [Description("Pix")]
    Pix = 1,
    [Description("Apenas contabilizada")]
    JustCount = 2
}
