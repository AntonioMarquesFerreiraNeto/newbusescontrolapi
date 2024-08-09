using BusesControl.Entities.Models.v1;

namespace BusesControl.Persistence.Repositories.Interfaces.v1;

public interface IUserRegistrationQueueRepository : IGenericRepository<UserRegistrationQueueModel>
{
    Task<IEnumerable<UserRegistrationQueueModel>> FindAsync(int page, int pageSize, string? search);
    Task<int> CountAsync(string? search = null);
    Task<UserRegistrationQueueModel?> GetByIdAsync(Guid id);
    Task<UserRegistrationQueueModel?> GetByUserWithEmployeeAsync(Guid userId);
    Task<UserRegistrationQueueModel?> GetByEmployeeAttributesAsync(string email, string cpf, DateOnly birthDate);
    Task<bool> ExistsByEmployee(Guid employeeId);
}
