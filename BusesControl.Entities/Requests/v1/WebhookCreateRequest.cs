using BusesControl.Entities.Enums.v1;

namespace BusesControl.Entities.Requests.v1;

public class WebhookCreateRequest
{
    public string Name { get; set; } = default!;
    public string Url { get; set; } = default!;
    public string Email { get; set; } = default!;
    public WebhookTypeEnum Type { get; set; }
    public IEnumerable<string> Events { get; set; } = default!;
}
