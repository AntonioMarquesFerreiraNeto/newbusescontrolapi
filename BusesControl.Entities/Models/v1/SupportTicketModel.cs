using BusesControl.Entities.Enums.v1;
using System.ComponentModel.DataAnnotations;

namespace BusesControl.Entities.Models.v1;

public class SupportTicketModel
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
    public SupportTicketStatusEnum Status { get; set; } = SupportTicketStatusEnum.Open;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdateAt { get; set; }
    public virtual ICollection<SupportTicketMessageModel> SupportTicketMessages { get; set; } = default!;
}
