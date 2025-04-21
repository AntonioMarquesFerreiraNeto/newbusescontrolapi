using System.ComponentModel.DataAnnotations;

namespace BusesControl.Entities.Models.v1;

public class ContactModel
{
    public Guid Id { get; set; }
    [MaxLength(150)]
    public string Name { get; set; } = default!;
    [MaxLength(150)]
    public string PhoneNumber { get; set; } = default!;
    [MaxLength(150)]
    public string Email { get; set; } = default!;
    [MaxLength(2000)]
    public string Message { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
}
