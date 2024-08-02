using BusesControl.Persistence.Contexts;
using BusesControl.Persistence.Repositories.Interfaces;

namespace BusesControl.Persistence.Repositories;

public class GenericRepository<T>(
    AppDbContext context
) : IGenericRepository<T> where T : class
{
    private readonly AppDbContext _context = context;

    public async Task AddAsync(T record)
    {
        await _context.AddAsync(record);
    }

    public async Task AddRangeAsync(IEnumerable<T> records)
    {
        await _context.AddRangeAsync(records);
    }

    public void Update(T record)
    {
        _context.Update(record);
    }

    public void UpdateRange(IEnumerable<T> records)
    {
        _context.UpdateRange(records);
    }

    public void Remove(T record)
    {
        _context.Remove(record);
    }

    public void RemoveRange(IEnumerable<T> records)
    {
        _context.RemoveRange(records);
    }
}
