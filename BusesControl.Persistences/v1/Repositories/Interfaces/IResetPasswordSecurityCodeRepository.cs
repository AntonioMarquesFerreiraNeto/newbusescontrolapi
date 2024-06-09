using BusesControl.Entities.Models;

namespace BusesControl.Persistence.v1.Repositories.Interfaces;

public interface IResetPasswordSecurityCodeRepository
{
    Task<ResetPasswordSecurityCodeModel?> GetByUserAsync(Guid userId);
    Task<ResetPasswordSecurityCodeModel?> GetByCodeAsync(string code);
    Task<bool> Create(ResetPasswordSecurityCodeModel record);
    bool Remove(ResetPasswordSecurityCodeModel record);
    Task<bool> ExistsByCode(string code);
}
