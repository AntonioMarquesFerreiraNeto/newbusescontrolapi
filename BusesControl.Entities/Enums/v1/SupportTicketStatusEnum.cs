using System.ComponentModel;

namespace BusesControl.Entities.Enums.v1;

public enum SupportTicketStatusEnum
{
    [Description("Aberto")]
    Open = 1,
    [Description("Em andamento")]
    InProgress = 2,
    [Description("Fechado")]
    Closed = 3,
    [Description("Cancelado")]
    Canceled = 4
}
