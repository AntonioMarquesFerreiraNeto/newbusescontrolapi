using System.ComponentModel;

namespace BusesControl.Entities.Enums;

public enum AvailabilityEnum
{
    [Description("Disponível")]
    Available = 0,

    [Description("Indisponível")]
    Unavailable = 1
}
