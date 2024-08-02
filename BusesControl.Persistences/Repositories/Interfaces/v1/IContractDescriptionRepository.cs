using BusesControl.Entities.Models.v1;

namespace BusesControl.Persistence.Repositories.Interfaces.v1;

public interface IContractDescriptionRepository : IGenericRepository<ContractDescriptionModel>
{
    Task<ContractDescriptionModel?> GetByIdAsync(Guid id);
    Task<IEnumerable<ContractDescriptionModel>> FindAsync(int page = 0, int pageSize = 0);
    Task<bool> ExitsAsync(Guid id);
    Task<bool> ExitsByReferenceAsync(string reference);
}
