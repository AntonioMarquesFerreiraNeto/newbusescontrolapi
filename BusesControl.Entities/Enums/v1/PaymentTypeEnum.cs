using System.ComponentModel;

namespace BusesControl.Entities.Enums.v1;

public enum PaymentTypeEnum
{
    [Description("Fatura única")]
    Single = 1,
    [Description("Fatura múltipla")]
    Multiple = 2
}
