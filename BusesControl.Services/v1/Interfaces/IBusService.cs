using BusesControl.Entities.Models;
using BusesControl.Entities.Request;

namespace BusesControl.Services.v1.Interfaces;

public interface IBusService
{
    Task<BusModel> GetByIdAsync(Guid id);
    Task<bool> CreateAsync(BusCreateRequest request);
    Task<bool> UpdateAsync(Guid id, BusUpdateRequest request);
}
