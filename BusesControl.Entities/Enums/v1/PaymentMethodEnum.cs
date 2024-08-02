using System.ComponentModel;

namespace BusesControl.Entities.Enums.v1;

public enum PaymentMethodEnum
{
    [Description("Cartão de crédito")]
    CreditCard = 1,
    [Description("Pix")]
    Pix = 2,
    [Description("Apenas contabilizada")]
    JustCount = 3
}
