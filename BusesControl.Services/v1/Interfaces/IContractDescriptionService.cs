using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Requests.v1;
using BusesControl.Entities.Responses.v1;

namespace BusesControl.Services.v1.Interfaces;

public interface IContractDescriptionService
{
    Task<PaginationResponse<ContractDescriptionModel>> FindAsync(PaginationRequest request);
    Task<ContractDescriptionModel> GetByIdAsync(Guid id);
    Task<bool> CreateAsync(ContractDescriptionCreateRequest request);
    Task<bool> UpdateAsync(Guid id, ContractDescriptionUpdateRequest request);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> DeleteAsync(Guid id);
}
