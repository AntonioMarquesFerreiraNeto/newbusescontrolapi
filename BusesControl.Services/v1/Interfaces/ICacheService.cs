using Microsoft.Extensions.Caching.Distributed;

namespace BusesControl.Services.v1.Interfaces;

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key);
    Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions options);
    Task<bool> RemoveAsync(string key);
}
