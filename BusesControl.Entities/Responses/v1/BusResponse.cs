using BusesControl.Entities.Enums.v1;

namespace BusesControl.Entities.Responses.v1;

public class BusResponse
{
    public Guid Id { get; set; }
    public Guid ColorId { get; set; } = default!;
    public string Brand { get; set; } = default!;
    public string Name { get; set; } = default!;
    public DateOnly ManufactureDate { get; set; }
    public string Renavam { get; set; } = default!;
    public string LicensePlate { get; set; } = default!;
    public string Chassi { get; set; } = default!;
    public int SeatingCapacity { get; set; }
    public BusStatusEnum Status { get; set; }
    public AvailabilityEnum Availability { get; set; }
}
