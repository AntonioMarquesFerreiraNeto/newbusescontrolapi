using BusesControl.Entities.Requests.v1;
using BusesControl.Entities.Responses.v1;

namespace BusesControl.Services.v1.Interfaces;

public interface ISupportTicketMessageService
{
    Task<IEnumerable<SupportTicketMessageResponse>> FindByTicketAsync(Guid ticketId);
    Task<bool> CreateInternalAsync(Guid ticketId, string message);
    Task<SupportTicketMessageResponse> CreateAsync(Guid ticketId, SupportTicketMessageCreateRequest request);
}
