using System.ComponentModel;

namespace BusesControl.Entities.Enums;

public enum ContractStatusEnum
{
    [Description("Aguardando Revisão")]
    WaitingReview = 1,
    [Description("Negado")]
    Denied = 2,
    [Description("Colhendo assinaturas")]
    WaitingSignature = 3,
    [Description("Em andamento")]
    InProgress = 4,
    [Description("Concluído")]
    Completed = 5
}
