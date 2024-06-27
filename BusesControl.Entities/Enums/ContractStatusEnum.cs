﻿using System.ComponentModel;

namespace BusesControl.Entities.Enums;

public enum ContractStatusEnum
{
    [Description("Aguardando Revisão")]
    WaitingReview = 1,
    [Description("Negado")]
    Denied = 2,
    [Description("Em andamento")]
    InProgress = 3,
    [Description("Concluído")]
    Completed = 4
}