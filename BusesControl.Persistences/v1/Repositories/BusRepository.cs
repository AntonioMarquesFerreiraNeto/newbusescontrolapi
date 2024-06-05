using BusesControl.Entities.Models;
using BusesControl.Persistence.Contexts;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Persistence.v1.Repositories;

public class BusRepository(
    AppDbContext _context
) : IBusRepository
{
    public async Task<IEnumerable<BusModel>> FindBySearchAsync(int pageSize, int pageNumber, string? search = null)
    {
        var query = _context.Buses.AsNoTracking();

        if (search is not null)
        {
            query = query.Where(x => x.Name.Contains(search) || x.Brand.Contains(search) || x.Chassi.Contains(search) || x.Color.Contains(search) || x.LicensePlate.Contains(search));
        }

        query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        var records = await query.ToListAsync();

        return records;
    }

    public async Task<BusModel?> GetByIdAsync(Guid id)
    {
        return await _context.Buses.SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> CreateAsync(BusModel bus)
    {
        await _context.Buses.AddAsync(bus);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UpdateAsync(BusModel bus)
    {
        _context.Buses.Update(bus);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ExistsByRenavamOrLicensePlateOrChassisAsync(string renavam, string licensePlate, string chassi, Guid? id = null)
    {
        return await _context.Buses.AnyAsync(x =>
            (x.Renavam == renavam ||
            x.LicensePlate == licensePlate ||
            x.Chassi == chassi) && x.Id != id
        );
    }
}
