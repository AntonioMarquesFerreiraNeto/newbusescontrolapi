namespace BusesControl.Entities.Models.v1;

public class SupportTicketMessageModel
{
    public Guid Id { get; set; }
    public Guid SupportTicketId { get; set; }
    public SupportTicketModel SupportTicket { get; set; } = default!;
    public Guid? SupportAgentId { get; set; }
    public EmployeeModel? SupportAgent { get; set; }
    public string Message { get; set; } = default!;
    public bool Delivered { get; set; }
    public bool IsSupportAgent { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
