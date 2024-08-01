using System.ComponentModel;

namespace BusesControl.Entities.Enums;

public enum SupportTicketTypeEnum
{
    [Description("Técnico")]
    Technical = 1,
    [Description("Negócio")]
    Business = 2,
    [Description("Financeiro")]
    Financial = 3,
    [Description("Outro")]
    Others = 4
}
