namespace BusesControl.Entities.Responses.v1;

public class SupportTicketMessageResponse
{
    public Guid Id { get; set; }
    public Guid SupportTicketId { get; set; }
    public Guid? SupportAgentId { get; set; }
    public EmployeeResponse? SupportAgent { get; set; }
    public string Message { get; set; } = default!;
    public bool IsSupportAgent { get; set; }
    public DateTime CreatedAt { get; set; }
}