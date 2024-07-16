using BusesControl.Entities.Models;

namespace BusesControl.Persistence.v1.Repositories.Interfaces;

public interface ITerminationRepository
{
    Task<IEnumerable<TerminationModel>> FindByContractAsync(Guid contractId, string? search = null);
    Task<TerminationModel?> GetByIdAsync(Guid? id);
    Task<bool> CreateAsync(TerminationModel record);
    bool Update(TerminationModel record);
    Task<bool> ExistsByContractAndCustomerAsync(Guid contractId, Guid customerId);
}
