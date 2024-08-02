using BusesControl.Entities.Models.v1;

namespace BusesControl.Persistence.Repositories.Interfaces.v1;

public interface ISavedCardRepository : IGenericRepository<SavedCardModel>
{
    Task<SavedCardModel?> GetByCustomerAsync(Guid customerId);
}
