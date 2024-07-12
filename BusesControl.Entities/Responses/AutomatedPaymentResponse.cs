namespace BusesControl.Entities.Responses;

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
