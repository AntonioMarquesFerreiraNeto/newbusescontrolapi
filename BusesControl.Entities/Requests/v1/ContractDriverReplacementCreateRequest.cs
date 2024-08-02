namespace BusesControl.Entities.Requests.v1;

public class ContractDriverReplacementCreateRequest
{
    public Guid DriverId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly TerminateDate { get; set; }
    public string ReasonDescription { get; set; } = default!;
}
