namespace BusesControl.Entities.Responses.v1;

public class SuccessResponse
{
    public string Message { get; set; } = default!;

    public SuccessResponse(string message)
    {
        Message = message;
    }
}
