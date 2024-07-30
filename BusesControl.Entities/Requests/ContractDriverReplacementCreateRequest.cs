namespace BusesControl.Entities.Requests;

public class ContractDriverReplacementCreateRequest
{
    public Guid DriverId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly TerminateDate { get; set; }
    public string ReasonDescription { get; set; } = default!;
}
