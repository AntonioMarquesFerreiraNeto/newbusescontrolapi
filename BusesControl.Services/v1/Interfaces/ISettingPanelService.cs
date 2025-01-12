using BusesControl.Entities.Enums.v1;
using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Requests.v1;
using BusesControl.Entities.Responses.v1;

namespace BusesControl.Services.v1.Interfaces;

public interface ISettingPanelService
{
    Task<SettingPanelModel> GetByIdAsync(Guid id);
    Task<PaginationResponse<SettingPanelModel>> FindAsync(PaginationRequest request);
    Task<IEnumerable<SettingPanelModel>> FindByParentAsync(SettingPanelParentEnum parent);
    Task<bool> CreateAsync(SettingPanelCreateRequest request);
    Task<bool> UpdateAsync(Guid id, SettingPanelUpdateRequest request);
    Task<bool> DeleteAsync(Guid id);
}
