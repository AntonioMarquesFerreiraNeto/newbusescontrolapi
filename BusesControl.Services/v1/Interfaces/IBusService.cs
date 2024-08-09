using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Requests.v1;
using BusesControl.Entities.Responses.v1;

namespace BusesControl.Services.v1.Interfaces;

public interface IBusService
{
    Task<PaginationResponse<BusModel>> FindBySearchAsync(PaginationRequest request);
    Task<BusModel> GetByIdAsync(Guid id);
    Task<bool> CreateAsync(BusCreateRequest request);
    Task<bool> UpdateAsync(Guid id, BusUpdateRequest request);
    Task<bool> ToggleActiveAsync(Guid id);
    Task<bool> ToggleAvailabilityAsync(Guid id);
}
