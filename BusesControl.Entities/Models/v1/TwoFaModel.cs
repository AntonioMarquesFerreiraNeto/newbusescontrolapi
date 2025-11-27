using System.ComponentModel.DataAnnotations;

namespace BusesControl.Entities.Models.v1;

public class TwoFaModel
{
    public Guid Id { get; set; }
    [MaxLength(8)]
    public string Code { get; set; } = default!;
    [MaxLength(32)]
    public string? Token { get; set; }
    [MaxLength(255)]
    public string UserEmail { get; set; } = default!;
    [MaxLength(255)]
    public string IpLocation { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public DateTime ExpirationAt { get; set; }
    public bool Used { get; set; }
}