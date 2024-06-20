using BusesControl.Entities.Models;
using BusesControl.Persistence.Contexts;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Persistence.v1.Repositories;

public class ResetPasswordSecurityCodeRepository(
    AppDbContext context
) : IResetPasswordSecurityCodeRepository
{
    private readonly AppDbContext _context = context;

    public async Task<ResetPasswordSecurityCodeModel?> GetByUserAsync(Guid userId)
    {
        return await _context.ResetPasswordsSecurityCode.AsNoTracking().SingleOrDefaultAsync(x => x.UserId == userId);
    }

    public async Task<ResetPasswordSecurityCodeModel?> GetByCodeAsync(string code)
    {
        return await _context.ResetPasswordsSecurityCode.AsNoTracking().SingleOrDefaultAsync(x => x.Code == code);
    }

    public async Task<bool> Create(ResetPasswordSecurityCodeModel record)
    {
        await _context.AddAsync(record);
        return true;
    }

    public bool Remove(ResetPasswordSecurityCodeModel record)
    {
        _context.Remove(record);
        return true;
    }

    public async Task<bool> ExistsByCode(string code)
    {
        return await _context.ResetPasswordsSecurityCode.AnyAsync(x => x.Code == code);
    }
}
