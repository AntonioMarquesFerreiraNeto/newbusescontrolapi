using BusesControl.Entities.Models.v1;

namespace BusesControl.Persistence.Repositories.Interfaces.v1;

public interface IUserRepository : IGenericRepository<UserModel>
{
    Task<UserModel?> GetByEmailAndCpfAndBirthDateAsync(string email, string cpf, DateOnly birthDate);
    Task<UserModel?> GetByIdWithEmployeeAsync(Guid id);
}
