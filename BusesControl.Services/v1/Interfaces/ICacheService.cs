namespace BusesControl.Services.v1.Interfaces;

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key);
    Task SetAsync<T>(string key, T value, TimeSpan time);
    Task<bool> RemoveAsync(string key);
}
