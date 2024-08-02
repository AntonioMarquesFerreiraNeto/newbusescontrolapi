using System.ComponentModel;

namespace BusesControl.Entities.Enums.v1;

public enum AvailabilityEnum
{
    [Description("Disponível")]
    Available = 1,

    [Description("Indisponível")]
    Unavailable = 2
}
