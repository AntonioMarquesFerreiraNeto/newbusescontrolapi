using System.ComponentModel;

namespace BusesControl.Entities.Enums.v1;

public enum EmployeeTypeEnum
{
    [Description("Motorista")]
    Driver = 1,
    [Description("Assistente")]
    Assistant = 2,
    [Description("Administrador")]
    Admin = 3,
    [Description("Agente de suporte")]
    SupportAgent = 4
}
