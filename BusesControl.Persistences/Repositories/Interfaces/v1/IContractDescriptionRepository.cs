using BusesControl.Entities.Models.v1;

namespace BusesControl.Persistence.Repositories.Interfaces.v1;

public interface IContractDescriptionRepository : IGenericRepository<ContractDescriptionModel>
{
    Task<IEnumerable<ContractDescriptionModel>> FindAsync(int page = 0, int pageSize = 0);
    Task<int> CountAsync();
    Task<ContractDescriptionModel?> GetByIdAsync(Guid id);
    Task<bool> ExitsAsync(Guid id);
    Task<bool> ExitsByReferenceAsync(string reference);
}
