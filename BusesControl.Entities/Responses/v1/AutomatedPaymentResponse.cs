using BusesControl.Entities.DTOs;

namespace BusesControl.Entities.Responses.v1;

public class AutomatedPaymentResponse
{
    public bool Success { get; private set; }
    public string? MessageFailure { get; private set; }
    public InvoicePayWithCardInAssasDTO? Content { get; private set; }

    public static AutomatedPaymentResponse Ok(InvoicePayWithCardInAssasDTO? content = null)
    {
        return new AutomatedPaymentResponse() 
        { 
            Success = true,
            Content = content,
        };
    }

    public static AutomatedPaymentResponse Fail(string? message)
    {
        return new AutomatedPaymentResponse()
        {
            Success = false,
            MessageFailure = message,
        };
    }
}
