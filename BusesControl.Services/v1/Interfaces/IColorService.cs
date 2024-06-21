using BusesControl.Entities.Models;
using BusesControl.Entities.Requests;

namespace BusesControl.Services.v1.Interfaces;

public interface IColorService
{
    Task<ColorModel> GetByIdAsync(Guid id);
    Task<IEnumerable<ColorModel>> FindBySearchAsync(int page, int pageSize, string? search = null);
    Task<bool> CreateAsync(ColorCreateRequest request);
    Task<bool> UpdateAsync(Guid id, ColorUpdateRequest request);
    Task<bool> ToggleActiveAsync(Guid id);
    Task<bool> DeleteAsync(Guid id);
}
