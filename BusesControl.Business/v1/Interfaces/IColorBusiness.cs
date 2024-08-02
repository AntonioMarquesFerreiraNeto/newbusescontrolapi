using BusesControl.Entities.Models.v1;

namespace BusesControl.Business.v1.Interfaces;

public interface IColorBusiness
{
    Task<bool> ValidateActiveAsync(Guid id);
    Task<bool> ExistsAsync(string color, Guid? id = null);
    Task<ColorModel> GetForDeleteAsync(Guid id);
}
