using BusesControl.Entities.Models.v1;

namespace BusesControl.Persistence.Repositories.Interfaces.v1;

public interface IUserRepository : IGenericRepository<UserModel>
{
    Task<IEnumerable<UserModel>> FindBySearchAsync(int page, int pageSize, string? search = null);
    Task<int> CountBySearchAsync(string? search = null);
    Task<UserModel?> GetByEmailAndCpfAndBirthDateAsync(string email, string cpf, DateOnly birthDate);
    Task<UserModel?> GetByIdWithEmployeeAsync(Guid id);
}
