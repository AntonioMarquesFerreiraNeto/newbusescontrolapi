using BusesControl.Entities.Models;

namespace BusesControl.Persistence.v1.Repositories.Interfaces;

public interface IEmployeeRepository
{
    Task<IEnumerable<EmployeeModel>> FindBySearchAsync(int page = 0, int pageSize = 0, string? search = null);
    Task<EmployeeModel?> GetByIdAsync(Guid id);
    Task<bool> CreateAsync(EmployeeModel record);
    bool Update(EmployeeModel record);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> ExistsByEmailOrPhoneNumberOrCpfAsync(string email, string phoneNumber, string cpf, Guid? id = null);
}
