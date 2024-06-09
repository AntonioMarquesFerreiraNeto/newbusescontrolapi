using BusesControl.Entities.Models;

namespace BusesControl.Persistence.v1.Repositories.Interfaces;

public interface IUserRegistrationSecurityCodeRepository
{
    Task<UserRegistrationSecurityCodeModel?> GetByUserAsync(Guid userId);
    Task<UserRegistrationSecurityCodeModel?> GetByCodeAsync(string code);
    Task<bool> CreateAsync(UserRegistrationSecurityCodeModel record);
    bool Remove(UserRegistrationSecurityCodeModel record);
    Task<bool> ExistsByCode(string code);
}
