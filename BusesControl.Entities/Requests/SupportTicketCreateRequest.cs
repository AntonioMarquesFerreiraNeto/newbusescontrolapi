using BusesControl.Entities.Enums;

namespace BusesControl.Entities.Requests;

public class SupportTicketCreateRequest
{
    public SupportTicketTypeEnum Type { get; set; }
    public string Title { get; set; } = default!;
    public string Message { get; set; } = default!;
}
