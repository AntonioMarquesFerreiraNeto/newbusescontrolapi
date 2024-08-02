namespace BusesControl.Entities.Responses.v1;

public class AutomatedPaymentResponse
{
    public bool Success { get; set; }
    public string? MessageFailure { get; set; }

    public AutomatedPaymentResponse(bool sucess = false, string? messageFailure = null)
    {
        Success = sucess;
        MessageFailure = messageFailure;
    }
}
