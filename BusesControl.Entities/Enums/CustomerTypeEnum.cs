using System.ComponentModel;

namespace BusesControl.Entities.Enums;

public enum CustomerTypeEnum
{
    [Description("Pessoa física")]
    NaturalPerson = 1,
    [Description("Pessoa jurídica")]
    LegalEntity = 2
}
