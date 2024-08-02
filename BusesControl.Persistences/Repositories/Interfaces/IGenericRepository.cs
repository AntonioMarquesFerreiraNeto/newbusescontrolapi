namespace BusesControl.Persistence.Repositories.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task AddAsync(T record);
    Task AddRangeAsync(IEnumerable<T> records);
    void Update(T record);
    void UpdateRange(IEnumerable<T> records);
    void Remove(T record);
    void RemoveRange(IEnumerable<T> records);
}
