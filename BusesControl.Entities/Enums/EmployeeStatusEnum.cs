using System.ComponentModel;

namespace BusesControl.Entities.Enums;

public enum EmployeeStatusEnum
{
    [Description("Ativo")]
    Active = 1,
    [Description("Inativo")]
    Inactive = 2
}
