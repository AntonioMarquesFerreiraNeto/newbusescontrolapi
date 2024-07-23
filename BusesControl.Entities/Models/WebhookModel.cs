using BusesControl.Entities.Enums;

namespace BusesControl.Entities.Models;

public class WebhookModel
{
    public Guid Id { get; set; }
    public string ExternalId { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Url { get; set; } = default!;
    public string SendType { get; set; } = default!;
    public bool Enabled { get; set; }
    public bool Interrupted { get; set; }
    public string Events { get; set; } = default!;
    public string AuthToken { get; set; } = default!;
    public int ApiVersion { get; set; }
    public WebhookTypeEnum Type { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
