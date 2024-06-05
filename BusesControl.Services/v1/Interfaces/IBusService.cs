using BusesControl.Entities.Models;
using BusesControl.Entities.Request;

namespace BusesControl.Services.v1.Interfaces;

public interface IBusService
{
    Task<IEnumerable<BusModel>> FindBySearchAsync(int pageSize, int pageNumber, string? search = null);
    Task<BusModel> GetByIdAsync(Guid id);
    Task<bool> CreateAsync(BusCreateRequest request);
    Task<bool> UpdateAsync(Guid id, BusUpdateRequest request);
    Task<bool> ToggleActiveAsync(Guid id);
    Task<bool> ToggleAvailabilityAsync(Guid id);
}
