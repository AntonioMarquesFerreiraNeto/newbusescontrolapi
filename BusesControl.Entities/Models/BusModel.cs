using BusesControl.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace BusesControl.Entities.Models;

public class BusModel
{
    public Guid Id { get; set; }
    public Guid ColorId { get; set; } = default!;
    public ColorModel Color { get; set; } = default!;
    [MaxLength(60)]
    public string Brand { get; set; } = default!;
    [MinLength(60)]
    public string Name { get; set; } = default!;
    public DateOnly ManufactureDate { get; set; }
    [MaxLength(11)]
    public string Renavam { get; set; } = default!;
    [MaxLength(7)]
    public string LicensePlate { get; set; } = default!;
    [MaxLength(17)]
    public string Chassi { get; set; } = default!;
    public int SeatingCapacity { get; set; }
    public BusStatusEnum Status { get; set; } = BusStatusEnum.Active;
    public AvailabilityEnum Availability { get; set; } = AvailabilityEnum.Available;
}
