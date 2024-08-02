using BusesControl.Entities.Models.v1;

namespace BusesControl.Persistence.Repositories.Interfaces.v1;

public interface ITerminationRepository : IGenericRepository<TerminationModel>
{
    Task<IEnumerable<TerminationModel>> FindByContractAsync(Guid contractId, string? search = null);
    Task<TerminationModel?> GetByIdAsync(Guid? id);
    Task<bool> ExistsByContractAndCustomerAsync(Guid contractId, Guid customerId);
}
