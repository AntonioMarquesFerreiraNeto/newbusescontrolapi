using BusesControl.Entities.Models.v1;

namespace BusesControl.Persistence.Repositories.Interfaces.v1;

public interface ISupportTicketMessageRepository : IGenericRepository<SupportTicketMessageModel>
{
    Task<IEnumerable<SupportTicketMessageModel>> FindByTicketAsync(Guid ticketId);
    Task MarkMessageAsDeliveredAsync(Guid id, bool isSupportAgent);
}
