using BusesControl.Entities.Models.v1;
using BusesControl.Persistence.Contexts;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Persistence.Repositories.v1;

public class TwoFaRepository: GenericRepository<TwoFaModel>, ITwoFaRepository
{
    private readonly AppDbContext _context;

    public TwoFaRepository(AppDbContext context) : base(context)
    { 
        _context = context;
    }

    public async Task<TwoFaModel?> GetByUserAsync(string email, string ipLocation)
        => await _context.TwoFas.OrderByDescending(x => x.CreatedAt).FirstOrDefaultAsync(x => x.UserEmail == email && x.IpLocation == ipLocation);

    public async Task<bool> ExistsAsync(string code) 
        => await _context.TwoFas.AnyAsync(x => x.Code == code);

    public async Task<bool> ExistsTokenValidAsync(string email, string ipLocation)
        => await _context.TwoFas.AnyAsync(x => x.UserEmail == email && x.Used && x.IpLocation == ipLocation && x.CreatedAt.Date == DateTime.UtcNow.Date);

    public async Task<TwoFaModel?> GetByCodeAsync(string code)
        => await _context.TwoFas.FirstOrDefaultAsync(x => x.Code == code && !x.Used);

    public async Task<TwoFaModel?> GetByTokenAsync(string? token)
        => await _context.TwoFas.OrderByDescending(x => x.CreatedAt).FirstOrDefaultAsync(x => x.Token == token && !x.Used);
}
