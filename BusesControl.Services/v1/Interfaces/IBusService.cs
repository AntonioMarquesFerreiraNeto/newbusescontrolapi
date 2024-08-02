using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Requests.v1;

namespace BusesControl.Services.v1.Interfaces;

public interface IBusService
{
    Task<IEnumerable<BusModel>> FindBySearchAsync(int page = 0, int pageSize = 0, string? search = null);
    Task<BusModel> GetByIdAsync(Guid id);
    Task<bool> CreateAsync(BusCreateRequest request);
    Task<bool> UpdateAsync(Guid id, BusUpdateRequest request);
    Task<bool> ToggleActiveAsync(Guid id);
    Task<bool> ToggleAvailabilityAsync(Guid id);
}
