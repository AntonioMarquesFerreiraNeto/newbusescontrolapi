using BusesControl.Entities.Models;

namespace BusesControl.Persistence.v1.Repositories.Interfaces;

public interface IBusRepository
{
    Task<IEnumerable<BusModel>> FindBySearchAsync(int page = 0, int pageSize = 0, string? search = null);
    Task<BusModel?> GetByIdAsync(Guid id);
    Task<bool> CreateAsync(BusModel bus);
    bool Update(BusModel bus);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> ExistsByRenavamOrLicensePlateOrChassisAsync(string renavam, string licensePlate, string chassi, Guid? id = null);
    Task<bool> ExistsByColorAsync(Guid colorId);
}
