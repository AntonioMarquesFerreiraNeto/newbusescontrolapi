using System.ComponentModel.DataAnnotations;

namespace BusesControl.Entities.Models.v1;

public class FeatureFlagModel
{
    public Guid Id { get; set; }
    [MaxLength(355)]
    public string Key { get; set; } = default!;
    [MaxLength(355)]
    public string Name { get; set; } = default!;
    public bool Enabled { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? Expiration { get; set; }

    public bool FeatureFlagEnabled()
    {
        return Enabled && (!Expiration.HasValue || Expiration > DateTime.Now); 
    }
}
