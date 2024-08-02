using BusesControl.Entities.Models.v1;

namespace BusesControl.Persistence.Repositories.Interfaces.v1;

public interface IResetPasswordSecurityCodeRepository : IGenericRepository<ResetPasswordSecurityCodeModel>
{
    Task<ResetPasswordSecurityCodeModel?> GetByUserAsync(Guid userId);
    Task<ResetPasswordSecurityCodeModel?> GetByCodeAsync(string code);
    Task<bool> ExistsByCode(string code);
}
