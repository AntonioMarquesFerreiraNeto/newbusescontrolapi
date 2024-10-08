﻿using BusesControl.Entities.Models.v1;
using BusesControl.Persistence.Contexts;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Persistence.Repositories.v1;

public class BusRepository(
    AppDbContext context
) : GenericRepository<BusModel>(context), IBusRepository
{
    private readonly AppDbContext _context = context;

    public async Task<IEnumerable<BusModel>> FindBySearchAsync(int page = 0, int pageSize = 0, string? search = null)
    {
        var query = _context.Buses.Include(x => x.Color).AsNoTracking();

        if (search is not null)
        {
            query = query.Where(x => x.Name.Contains(search) || x.Brand.Contains(search) || x.Chassi.Contains(search) || x.Color.Color.Contains(search) || x.LicensePlate.Contains(search));
        }

        if (page > 0 & pageSize > 0)
        {
            query = query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        return await query.ToListAsync();
    }

    public async Task<int> CountBySearchAsync(string? search = null)
    {
        var query = _context.Buses.Include(x => x.Color).AsNoTracking();

        if (search is not null)
        {
            query = query.Where(x => x.Name.Contains(search) || x.Brand.Contains(search) || x.Chassi.Contains(search) || x.Color.Color.Contains(search) || x.LicensePlate.Contains(search));
        }

        return await query.CountAsync();
    }

    public async Task<BusModel?> GetByIdAsync(Guid id)
    {
        return await _context.Buses.Include(x => x.Color).SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Buses.AnyAsync(x => x.Id == id);
    }

    public async Task<bool> ExistsByRenavamOrLicensePlateOrChassisAsync(string renavam, string licensePlate, string chassi, Guid? id = null)
    {
        return await _context.Buses.AnyAsync(x =>
            (x.Renavam == renavam ||
            x.LicensePlate == licensePlate ||
            x.Chassi == chassi) && x.Id != id
        );
    }

    public async Task<bool> ExistsByColorAsync(Guid colorId)
    {
        return await _context.Buses.AnyAsync(x => x.ColorId == colorId);
    }
}
