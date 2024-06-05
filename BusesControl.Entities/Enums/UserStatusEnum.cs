﻿using System.ComponentModel;

namespace BusesControl.Entities.Enums;

public enum UserStatusEnum
{
    [Description("Ativo")]
    Active = 1,
    [Description("Inativo")]
    Inactive = 2
}
