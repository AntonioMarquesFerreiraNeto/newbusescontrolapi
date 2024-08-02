namespace BusesControl.Entities.Responses.v1;

public class SystemResponse
{
    public List<string> SuccessOperation { get; set; } = [];
    public List<string> FailureOperation { get; set; } = [];
    public string? NoOperation { get; set; }
}
