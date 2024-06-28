using BusesControl.Entities.Models;

namespace BusesControl.Persistence.v1.Repositories.Interfaces;

public interface ICustomerContractRepository
{
    Task<CustomerContractModel?> GetByIdAsync(Guid id);
    Task<CustomerContractModel?> GetByContractAndCustomerWithIncludesAsync(Guid contractId, Guid customerId);
    Task<IEnumerable<CustomerContractModel>> FindByContractAsync(Guid contractId);
    Task<bool> CreateRangeAsync(IEnumerable<CustomerContractModel> record);
    bool Update(CustomerContractModel record);
    bool RemoveRange(IEnumerable<CustomerContractModel> records);
}
