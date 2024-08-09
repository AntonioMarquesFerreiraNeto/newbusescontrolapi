using BusesControl.Entities.Models.v1;

namespace BusesControl.Persistence.Repositories.Interfaces.v1;

public interface IBusRepository : IGenericRepository<BusModel>
{
    Task<IEnumerable<BusModel>> FindBySearchAsync(int page = 0, int pageSize = 0, string? search = null);
    Task<int> CountBySearchAsync(string? search = null);
    Task<BusModel?> GetByIdAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> ExistsByRenavamOrLicensePlateOrChassisAsync(string renavam, string licensePlate, string chassi, Guid? id = null);
    Task<bool> ExistsByColorAsync(Guid colorId);
}
