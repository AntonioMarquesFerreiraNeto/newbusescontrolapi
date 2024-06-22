using BusesControl.Entities.Models;
using BusesControl.Entities.Requests;
using BusesControl.Entities.Responses;

namespace BusesControl.Services.v1.Interfaces;

public interface ISettingsPanelService
{
    Task<SettingsPanelResponse> GetByIdAsync(Guid id);
    Task<IEnumerable<SettingsPanelModel>> FindAsync(int page, int pageSize);
    Task<bool> CreateAsync(SettingsPanelCreateRequest request);
    Task<bool> UpdateAsync(Guid id, SettingsPanelUpdateRequest request);
    Task<bool> DeleteAsync(Guid id);
}
