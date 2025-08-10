using BusesControl.Entities.Models.v1;

namespace BusesControl.Persistence.Repositories.Interfaces.v1;

public interface IFeatureFlagRepository : IGenericRepository<FeatureFlagModel>
{
    Task<FeatureFlagModel?> GetByKeyAsync(string key);
}
