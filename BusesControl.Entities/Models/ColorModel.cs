using System.ComponentModel.DataAnnotations;

namespace BusesControl.Entities.Models;

public class ColorModel
{
    public Guid Id { get; set; }
    public bool Active { get; set; }
    [MaxLength(30)]
    public string Color { get; set; } = default!;
    public ICollection<BusModel> Buses { get; set; } = default!;
}
