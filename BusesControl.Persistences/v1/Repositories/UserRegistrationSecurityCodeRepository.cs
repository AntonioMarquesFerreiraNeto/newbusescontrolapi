using BusesControl.Entities.Models;
using BusesControl.Persistence.Contexts;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Persistence.v1.Repositories;

public class UserRegistrationSecurityCodeRepository(
    AppDbContext context
) : IUserRegistrationSecurityCodeRepository
{
    private readonly AppDbContext _context = context;

    public async Task<UserRegistrationSecurityCodeModel?> GetByCodeAsync(string code)
    {
        return await _context.UsersRegistrationSecurityCode.AsNoTracking().SingleOrDefaultAsync(x => x.Code == code);
    }

    public async Task<UserRegistrationSecurityCodeModel?> GetByUserAsync(Guid userId)
    {
        return await _context.UsersRegistrationSecurityCode.AsNoTracking().SingleOrDefaultAsync(x => x.UserId == userId);
    }

    public async Task<bool> CreateAsync(UserRegistrationSecurityCodeModel record)
    {
        await _context.UsersRegistrationSecurityCode.AddAsync(record);
        return true;
    }

    public bool Remove(UserRegistrationSecurityCodeModel record)
    {
        _context.UsersRegistrationSecurityCode.Remove(record);
        return true;
    }

    public async Task<bool> ExistsByCode(string code)
    {
        return await _context.UsersRegistrationSecurityCode.AnyAsync(x => x.Code == code);
    }
}
