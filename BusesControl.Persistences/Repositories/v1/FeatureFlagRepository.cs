using BusesControl.Entities.Models.v1;
using BusesControl.Persistence.Contexts;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Persistence.Repositories.v1;

public class FeatureFlagRepository(
    AppDbContext context
) : GenericRepository<FeatureFlagModel>(context), IFeatureFlagRepository
{
    private readonly AppDbContext _context = context;

    public async Task<FeatureFlagModel?> GetByKeyAsync(string key) 
        => await _context.FeatureFlags.FirstOrDefaultAsync(x => x.Key == key);
}
