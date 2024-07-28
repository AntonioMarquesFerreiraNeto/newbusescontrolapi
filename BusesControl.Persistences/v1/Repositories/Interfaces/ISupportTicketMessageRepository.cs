using BusesControl.Entities.Models;

namespace BusesControl.Persistence.v1.Repositories.Interfaces;

public interface ISupportTicketMessageRepository
{
    Task<bool> CreateAsync(SupportTicketMessageModel record);
}
