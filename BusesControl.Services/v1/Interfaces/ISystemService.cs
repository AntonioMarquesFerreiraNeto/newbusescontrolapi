using BusesControl.Entities.Responses;

namespace BusesControl.Services.v1.Interfaces;

public interface ISystemService
{
    Task<SystemResponse> AutomatedPaymentAsync(DateTime? dateNow = null);
    Task<SystemResponse> AutomatedOverdueInvoiceProcessingAsync();
}
