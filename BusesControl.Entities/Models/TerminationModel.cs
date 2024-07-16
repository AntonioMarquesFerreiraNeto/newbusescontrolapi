using BusesControl.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Entities.Models;

public class TerminationModel
{
    public Guid Id { get; set; }
    public Guid ContractId { get; set; }
    public ContractModel Contract { get; set; } = default!;
    public Guid FinancialId { get; set; }
    public FinancialModel Financial { get; set; } = default!;
    public Guid CustomerId { get; set; }
    public CustomerModel Customer { get; set; } = default!;
    [Precision(10, 2)]
    public decimal Price { get; set; }
    public TerminationStatusEnum Status { get; set; } = TerminationStatusEnum.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}