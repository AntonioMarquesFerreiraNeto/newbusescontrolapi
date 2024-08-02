using BusesControl.Entities.Models.v1;

namespace BusesControl.Persistence.Repositories.Interfaces.v1;

public interface ICustomerContractRepository : IGenericRepository<CustomerContractModel>
{
    Task<CustomerContractModel?> GetByIdAsync(Guid id);
    Task<CustomerContractModel?> GetByContractAndCustomerAsync(Guid contractId, Guid customerId);
    Task<CustomerContractModel?> GetByContractAndCustomerWithIncludesAsync(Guid contractId, Guid customerId);
    Task<IEnumerable<CustomerContractModel>> FindByContractAsync(Guid contractId);
    Task<IEnumerable<CustomerContractModel>> FindByProcessTerminationAsync(bool processTermination);
    Task<int> CountByContractAsync(Guid contractId);
}
