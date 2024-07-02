using BusesControl.Entities.Models;

namespace BusesControl.Persistence.v1.Repositories.Interfaces;

public interface IContractDescriptionRepository
{
    Task<ContractDescriptionModel?> GetByIdAsync(Guid id);
    Task<IEnumerable<ContractDescriptionModel>> FindAsync(int page = 0, int pageSize = 0);
    Task<bool> CreateAsync(ContractDescriptionModel record);
    bool Update(ContractDescriptionModel record);
    bool Delete(ContractDescriptionModel record);
    Task<bool> ExitsAsync(Guid id);
    Task<bool> ExitsByReferenceAsync(string reference);
}
