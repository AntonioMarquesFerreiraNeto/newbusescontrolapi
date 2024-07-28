using BusesControl.Entities.Enums;
using BusesControl.Entities.Models;
using System.ComponentModel.DataAnnotations;

namespace BusesControl.Entities.Responses;

public class SupportTicketResponse
{
    public Guid Id { get; set; }
    [MaxLength(8)]
    public string Reference { get; set; } = default!;
    public Guid EmployeeId { get; set; }
    public EmployeeModel Employee { get; set; } = default!;
    public Guid? SupportAgentId { get; set; }
    public EmployeeModel? SupportAgent { get; set; }
    public string Title { get; set; } = default!;
    public SupportTicketTypeEnum Type { get; set; }
    public SupportTicketStatusEnum Status { get; set; } 
    public DateTime CreatedAt { get; set; } 
    public DateTime? UpdateAt { get; set; }
    public virtual ICollection<SupportTicketMessageResponse> SupportTicketMessages { get; set; } = default!;
}
