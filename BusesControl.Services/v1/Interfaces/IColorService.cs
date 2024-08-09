using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Requests.v1;
using BusesControl.Entities.Responses.v1;

namespace BusesControl.Services.v1.Interfaces;

public interface IColorService
{
    Task<PaginationResponse<ColorModel>> FindBySearchAsync(PaginationRequest request);
    Task<ColorModel> GetByIdAsync(Guid id);
    Task<bool> CreateAsync(ColorCreateRequest request);
    Task<bool> UpdateAsync(Guid id, ColorUpdateRequest request);
    Task<bool> ToggleActiveAsync(Guid id);
    Task<bool> DeleteAsync(Guid id);
}
