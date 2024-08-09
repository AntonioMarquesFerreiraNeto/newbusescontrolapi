using BusesControl.Entities.Models.v1;
using BusesControl.Persistence.Contexts;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Persistence.Repositories.v1;

public class ColorRepository(
    AppDbContext context
) : GenericRepository<ColorModel>(context), IColorRepository
{
    private readonly AppDbContext _context = context;

    public async Task<IEnumerable<ColorModel>> FindBySearchAsync(int page = 0, int pageSize = 0, string? search = null)
    {
        var query = _context.Colors.AsNoTracking();

        if (search is not null)
        {
            query = query.Where(x => x.Color.Contains(search));
        }

        if (page >= 1 && pageSize >= 1)
        {
            query = query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        var records = await query.ToListAsync();

        return records;
    }

    public async Task<int> CountBySearchAsync(string? search = null)
    {
        var query = _context.Colors.AsNoTracking();

        if (search is not null)
        {
            query = query.Where(x => x.Color.Contains(search));
        }

        return await query.CountAsync();
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

    public async Task<bool> ExistsAsync(string color, Guid? id = null)
    {
        return await _context.Colors.AnyAsync(x => x.Color == color && x.Id != id);
    }
}
