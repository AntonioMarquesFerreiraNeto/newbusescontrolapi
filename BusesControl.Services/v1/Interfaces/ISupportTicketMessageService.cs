using BusesControl.Entities.Requests;

namespace BusesControl.Services.v1.Interfaces;

public interface ISupportTicketMessageService
{
    Task<bool> CreateInternalAsync(Guid ticketId, string message);
    Task<bool> CreateAsync(Guid ticketId, SupportTicketMessageCreateRequest request);
}
