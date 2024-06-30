namespace BusesControl.Entities.Responses;

public class CustomerContractResponse
{
    public Guid Id { get; set; }
    public Guid ContractId { get; set; }
    public Guid CustomerId { get; set; }
    public CustomerResponse Customer { get; set; } = default!;
    public bool ProcessTermination { get; set; }
    public DateTime? ProcessTerminationDate { get; set; }
}
