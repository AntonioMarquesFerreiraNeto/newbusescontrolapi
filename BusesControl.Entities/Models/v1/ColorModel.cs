using System.ComponentModel.DataAnnotations;

namespace BusesControl.Entities.Models.v1;

public class ColorModel
{
    public Guid Id { get; set; }
    [MaxLength(30)]
    public string Color { get; set; } = default!;
    public bool Active { get; set; } = true;
}
