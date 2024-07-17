namespace BusesControl.Entities.Models;

public class CustomerContractModel
{
    public Guid Id { get; set; }
    public Guid ContractId { get; set; }
    public ContractModel Contract { get; set; } = default!;
    public Guid CustomerId { get; set; }
    public CustomerModel Customer { get; set; } = default!;
    public bool ProcessTermination { get; set; }
    public DateTime? ProcessTerminationDate { get; set; }
    public bool Active { get; set; } = true;
}
