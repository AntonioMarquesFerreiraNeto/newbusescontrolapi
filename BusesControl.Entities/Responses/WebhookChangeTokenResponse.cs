namespace BusesControl.Entities.Responses;

public class WebhookChangeTokenResponse
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    
    public WebhookChangeTokenResponse(bool success, string? errorMessage = null)
    {
        Success = success;
        ErrorMessage = errorMessage;
    }
}
