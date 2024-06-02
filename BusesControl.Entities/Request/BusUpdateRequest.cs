﻿namespace BusesControl.Entities.Request;

public class BusUpdateRequest
{
    public string Brand { get; set; } = default!;
    public string Name { get; set; } = default!;
    public DateOnly ManufactureDate { get; set; } = default!;
    public string Renavam { get; set; } = default!;
    public string LicensePlate { get; set; } = default!;
    public string Chassi { get; set; } = default!;
    public int SeatingCapacity { get; set; }
    public string Color { get; set; } = default!;
}
