using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Requests.v1;
using BusesControl.Entities.Responses.v1;

namespace BusesControl.Services.v1.Interfaces;

public interface ICustomerService
{
    Task<PaginationResponse<CustomerModel>> FindBySearchAsync(PaginationRequest request);
    Task<CustomerModel> GetByIdAsync(Guid id);
    Task<bool> CreateAsync(CustomerCreateRequest request);
    Task<bool> UpdateAsync(Guid id, CustomerUpdateRequest request);
    Task<bool> ToggleActiveAsync(Guid id);
}
