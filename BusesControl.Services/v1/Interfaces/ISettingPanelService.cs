using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Requests.v1;

namespace BusesControl.Services.v1.Interfaces;

public interface ISettingPanelService
{
    Task<SettingPanelModel> GetByIdAsync(Guid id);
    Task<IEnumerable<SettingPanelModel>> FindAsync(int page, int pageSize);
    Task<bool> CreateAsync(SettingPanelCreateRequest request);
    Task<bool> UpdateAsync(Guid id, SettingPanelUpdateRequest request);
    Task<bool> DeleteAsync(Guid id);
}
