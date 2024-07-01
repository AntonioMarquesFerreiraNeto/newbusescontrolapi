using BusesControl.Entities.Models;
using BusesControl.Entities.Requests;

namespace BusesControl.Services.v1.Interfaces;

public interface IContractDescriptionService
{
    Task<ContractDescriptionModel> GetByIdAsync(Guid id);
    Task<IEnumerable<ContractDescriptionModel>> FindAsync(int page, int pageSize);
    Task<bool> CreateAsync(ContractDescriptionCreateRequest request);
    Task<bool> UpdateAsync(Guid id, ContractDescriptionUpdateRequest request);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> DeleteAsync(Guid id);
}
