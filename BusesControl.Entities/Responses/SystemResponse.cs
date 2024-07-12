namespace BusesControl.Entities.Responses;

public class SystemResponse
{
    public List<string> SuccessOperation { get; set; } = [];
    public List<string> FailureOperation { get; set; } = [];
    public string? NoOperation { get; set; }
}
