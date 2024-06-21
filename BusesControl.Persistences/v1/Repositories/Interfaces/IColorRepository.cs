using BusesControl.Entities.Models;

namespace BusesControl.Persistence.v1.Repositories.Interfaces;

public interface IColorRepository
{
    Task<ColorModel?> GetByIdAsync(Guid id);
    Task<IEnumerable<ColorModel>> FindBySearchAsync(int page = 0, int pageSize = 0, string? search = null);
    Task<bool> CreateAsync(ColorModel record);
    bool Update(ColorModel record);
    bool Remove(ColorModel record);
    Task<bool> ExistsAsync(string color, Guid? id = null);
}
