using BusesControl.Entities.Models;

namespace BusesControl.Persistence.v1.Repositories.Interfaces;

public interface IUserRegistrationQueueRepository
{
    Task<IEnumerable<UserRegistrationQueueModel>> FindAsync(int pageSize, int pageNumber, string? search);
    Task<UserRegistrationQueueModel?> GetByIdAsync(Guid id);
    Task<UserRegistrationQueueModel?> GetByUserAsync(Guid userId);
    Task<UserRegistrationQueueModel?> GetByEmployeeAttributesAsync(string email, string cpf, DateOnly birthDate);
    Task<bool> CreateAsync(UserRegistrationQueueModel record);
    bool Update(UserRegistrationQueueModel record);
    bool Delete(UserRegistrationQueueModel record);
    Task<bool> ExistsByEmployee(Guid employeeId);
}
