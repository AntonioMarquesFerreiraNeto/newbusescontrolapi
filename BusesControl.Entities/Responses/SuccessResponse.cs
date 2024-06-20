namespace BusesControl.Entities.Response;

public class SuccessResponse
{
    public string Message { get; set; } = default!;

    public SuccessResponse(string message)
    {
        Message = message;
    }
}
