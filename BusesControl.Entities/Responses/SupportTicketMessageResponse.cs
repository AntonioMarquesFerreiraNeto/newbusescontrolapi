using BusesControl.Entities.Models;

namespace BusesControl.Entities.Responses;

public class SupportTicketMessageResponse
{
    public Guid Id { get; set; }
    public Guid SupportTicketId { get; set; }
    public Guid? SupportAgentId { get; set; }
    public EmployeeModel? SupportAgent { get; set; }
    public string Message { get; set; } = default!;
    public bool IsSupportAgent { get; set; }
    public DateTime CreatedAt { get; set; }
}