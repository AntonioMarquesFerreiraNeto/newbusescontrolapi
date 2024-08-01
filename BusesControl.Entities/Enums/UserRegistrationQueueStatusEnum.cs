using System.ComponentModel;

namespace BusesControl.Entities.Enums;

public enum UserRegistrationQueueStatusEnum
{
    [Description("Iniciado")]
    Started = 1,
    [Description("Aguardando senha")]
    WaitingForPassword = 2,
    [Description("Finalizado")]
    Finished = 3,
    [Description("Aprovado")]
    Approved = 4
}
