using BusesControl.Entities.Models;

namespace BusesControl.Persistence.v1.Repositories.Interfaces;

public interface ICustomerContractRepository
{
    Task<CustomerContractModel> GetByIdAsync(Guid id);
    Task<bool> CreateAsync(CustomerContractModel record);
    bool Update(CustomerContractModel record);
    bool Delete(CustomerContractModel record);
}
