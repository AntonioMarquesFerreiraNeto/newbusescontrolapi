using BusesControl.Entities.Models.v1;
using BusesControl.Persistence.Contexts;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Persistence.Repositories.v1;

public class ResetPasswordSecurityCodeRepository(
    AppDbContext context
) : GenericRepository<ResetPasswordSecurityCodeModel>(context), IResetPasswordSecurityCodeRepository
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

    public async Task<bool> ExistsByCode(string code)
    {
        return await _context.ResetPasswordsSecurityCode.AnyAsync(x => x.Code == code);
    }
}
