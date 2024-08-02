using BusesControl.Entities.Enums.v1;

namespace BusesControl.Entities.Requests.v1;

public class SupportTicketCreateRequest
{
    public SupportTicketTypeEnum Type { get; set; }
    public string Title { get; set; } = default!;
    public string Message { get; set; } = default!;
}
