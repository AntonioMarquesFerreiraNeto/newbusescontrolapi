namespace BusesControl.Entities.Requests.v1;

public class ContractDescriptionUpdateRequest
{
    public string Owner { get; set; } = default!;
    public string GeneralProvisions { get; set; } = default!;
    public string Objective { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string SubTitle { get; set; } = default!;
    public string Copyright { get; set; } = default!;
}
