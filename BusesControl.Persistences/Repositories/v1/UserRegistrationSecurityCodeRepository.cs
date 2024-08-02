using BusesControl.Entities.Models.v1;
using BusesControl.Persistence.Contexts;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Persistence.Repositories.v1;

public class UserRegistrationSecurityCodeRepository(
    AppDbContext context
) : GenericRepository<UserRegistrationSecurityCodeModel>(context), IUserRegistrationSecurityCodeRepository
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

    public async Task<bool> ExistsByCode(string code)
    {
        return await _context.UsersRegistrationSecurityCode.AnyAsync(x => x.Code == code);
    }
}
