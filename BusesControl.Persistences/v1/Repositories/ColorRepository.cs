using BusesControl.Entities.Models;
using BusesControl.Persistence.Contexts;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Persistence.v1.Repositories;

public class ColorRepository(
    AppDbContext context
) : IColorRepository
{
    private readonly AppDbContext _context = context;

    public async Task<IEnumerable<ColorModel>> FindBySearchAsync(int page = 0, int pageSize = 0, string? color = null)
    {
        var query = _context.Colors.AsNoTracking();

        if (color is not null)
        {
            query = query.Where(x => x.Color.Contains(color));
        }

        if (page >= 1 && pageSize >= 1)
        {
            query = query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        var records = await query.ToListAsync();

        return records;
    }

    public async Task<ColorModel?> GetByIdAsync(Guid id)
    {
        return await _context.Colors.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> CreateAsync(ColorModel record)
    {
        await _context.Colors.AddAsync(record);
        return true;
    }

    public bool Update(ColorModel record)
    {
        _context.Colors.Update(record);
        return true;
    }

    public bool Remove(ColorModel record)
    {
        _context.Colors.Remove(record);
        return true;
    }
}
