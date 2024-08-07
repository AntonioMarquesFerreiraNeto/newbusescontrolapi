﻿using BusesControl.Entities.Enums.v1;

namespace BusesControl.Entities.Responses.v1;

public class WebhookResponse
{
    public Guid Id { get; set; }
    public string ExternalId { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Url { get; set; } = default!;
    public string SendType { get; set; } = default!;
    public bool Enabled { get; set; }
    public bool Interrupted { get; set; }
    public string AuthToken { get; set; } = default!;
    public int ApiVersion { get; set; }
    public WebhookTypeEnum Type { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public IEnumerable<string> Events { get; set; } = default!;
}
