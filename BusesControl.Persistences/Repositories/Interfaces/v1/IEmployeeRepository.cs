using BusesControl.Entities.Models.v1;

namespace BusesControl.Persistence.Repositories.Interfaces.v1;

public interface IEmployeeRepository : IGenericRepository<EmployeeModel>
{
    Task<IEnumerable<EmployeeModel>> FindBySearchAsync(int page = 0, int pageSize = 0, string? search = null);
    Task<int> CountBySearchAsync(string? search = null);
    Task<EmployeeModel?> GetByIdAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> ExistsByEmailOrPhoneNumberOrCpfAsync(string email, string phoneNumber, string cpf, Guid? id = null);
}
