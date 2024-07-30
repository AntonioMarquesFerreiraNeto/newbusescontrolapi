namespace BusesControl.Entities.Models;

public class ContractDriverReplacementModel
{
    public Guid Id { get; set; }
    public Guid ContractId { get; set; }
    public ContractModel Contract { get; set; } = default!;
    public Guid DriverId { get; set; }
    public EmployeeModel Driver { get; set; } = default!;
    public DateOnly StartDate { get; set; }
    public DateOnly TerminateDate { get; set; }
    public string ReasonDescription  { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
}
