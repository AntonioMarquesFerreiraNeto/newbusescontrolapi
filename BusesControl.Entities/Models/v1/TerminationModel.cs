using Microsoft.EntityFrameworkCore;

namespace BusesControl.Entities.Models.v1;

public class TerminationModel
{
    public Guid Id { get; set; }
    public Guid ContractId { get; set; }
    public ContractModel Contract { get; set; } = default!;
    public Guid CustomerId { get; set; }
    public CustomerModel Customer { get; set; } = default!;
    [Precision(10, 2)]
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}