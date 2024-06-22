using BusesControl.Entities.Models;

namespace BusesControl.Persistence.v1.Repositories.Interfaces;

public interface IContractRepository
{
    Task<ContractModel?> GetByIdAsync(Guid id);
    Task<ContractModel?> GetByIdWithIncludesAsync(Guid id);
    Task<IEnumerable<ContractModel>> FindAsync(int page = 0, int pageSize = 0);
    Task<bool> CreateAsync(ContractModel record);
    bool Update(ContractModel record);
    bool Delete(ContractModel record);
}
