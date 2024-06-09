using BusesControl.Entities.Models;

namespace BusesControl.Persistence.v1.Repositories.Interfaces;

public interface IResetUserRepository
{
    Task<ResetUserModel?> GetByUserAsync(Guid userId);
    Task<ResetUserModel?> GetByCodeAsync(string code);
    Task<bool> Create(ResetUserModel record);
    bool Remove(ResetUserModel record);
    Task<bool> ExistsByCode(string code);
}
