namespace BusesControl.Entities.Requests.v1;

public class ContractBusReplacementCreateRequest
{
    public Guid BusId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly TerminateDate { get; set; }
    public string ReasonDescription { get; set; } = default!;
}
