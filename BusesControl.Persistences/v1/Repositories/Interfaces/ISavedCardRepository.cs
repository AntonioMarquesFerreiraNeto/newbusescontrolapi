using BusesControl.Entities.Models;

namespace BusesControl.Persistence.v1.Repositories.Interfaces;

public interface ISavedCardRepository
{
    Task<SavedCardModel?> GetByCustomerAsync(Guid customerId);
    Task<bool> CreateAsync(SavedCardModel record);
}
