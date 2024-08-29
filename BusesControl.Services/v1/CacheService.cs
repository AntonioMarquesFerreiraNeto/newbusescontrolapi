using BusesControl.Services.v1.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;

namespace BusesControl.Services.v1;

public class CacheService(
    IDistributedCache _distributedCache
) : ICacheService
{
    public async Task<T?> GetAsync<T>(string key)
    {
        try
        {
            var cachedData = await _distributedCache.GetAsync(key);
            if (cachedData is null)
            {
                return default!;
            }

            var serializedData = Encoding.UTF8.GetString(cachedData);
            return JsonSerializer.Deserialize<T>(serializedData);
        }
        catch (Exception)
        {
            return default!;
        }
    }

    public async Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions options)
    {
        try 
        {
            var serializedData = JsonSerializer.Serialize(value);
            var encodedData = Encoding.UTF8.GetBytes(serializedData);
            await _distributedCache.SetAsync(key, encodedData, options);
        }
        catch (Exception) { }
    }

    public async Task<bool> RemoveAsync(string key)
    {
        try 
        {
            await _distributedCache.RemoveAsync(key);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
