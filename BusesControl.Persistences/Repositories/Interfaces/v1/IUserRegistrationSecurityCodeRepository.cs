using BusesControl.Entities.Models.v1;

namespace BusesControl.Persistence.Repositories.Interfaces.v1;

public interface IUserRegistrationSecurityCodeRepository : IGenericRepository<UserRegistrationSecurityCodeModel>
{
    Task<UserRegistrationSecurityCodeModel?> GetByCodeAsync(string code);
    Task<UserRegistrationSecurityCodeModel?> GetByUserAsync(Guid userId);
    Task<bool> ExistsByCode(string code);
}
