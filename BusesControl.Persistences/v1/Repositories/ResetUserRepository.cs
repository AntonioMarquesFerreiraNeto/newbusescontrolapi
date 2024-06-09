using BusesControl.Entities.Models;
using BusesControl.Persistence.Contexts;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Persistence.v1.Repositories;

public class ResetUserRepository(
    AppDbContext _context
) : IResetUserRepository
{
    public async Task<ResetUserModel?> GetByUserAsync(Guid userId)
    {
        return await _context.ResetsUser.SingleOrDefaultAsync(x => x.UserId == userId);
    }

    public async Task<ResetUserModel?> GetByCodeAsync(string code)
    {
        return await _context.ResetsUser.SingleOrDefaultAsync(x => x.Code == code);
    }

    public async Task<bool> Create(ResetUserModel record)
    {
        await _context.AddAsync(record);
        return true;
    }

    public bool Remove(ResetUserModel record)
    {
        _context.Remove(record);
        return true;
    }

    public async Task<bool> ExistsByCode(string code)
    {
        return await _context.ResetsUser.AnyAsync(x => x.Code == code);
    }
}
