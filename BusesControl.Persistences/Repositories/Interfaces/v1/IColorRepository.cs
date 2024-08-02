using BusesControl.Entities.Models.v1;

namespace BusesControl.Persistence.Repositories.Interfaces.v1;

public interface IColorRepository : IGenericRepository<ColorModel>
{
    Task<ColorModel?> GetByIdAsync(Guid id);
    Task<IEnumerable<ColorModel>> FindBySearchAsync(int page = 0, int pageSize = 0, string? search = null);
    Task<bool> ExistsAsync(string color, Guid? id = null);
}
