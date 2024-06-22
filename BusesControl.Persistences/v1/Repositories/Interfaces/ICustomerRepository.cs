using BusesControl.Entities.Models;

namespace BusesControl.Persistence.v1.Repositories.Interfaces;

public interface ICustomerRepository
{
    Task<IEnumerable<CustomerModel>> FindBySearchAsync(int pageSize, int pageNumber, string? search = null);
    Task<CustomerModel?> GetByIdAsync(Guid id);
    Task<bool> CreateAsync(CustomerModel record);
    bool Update(CustomerModel record);
    Task<bool> ExistsByEmailOrPhoneNumberOrCpfOrCnpjAsync(string email, string phoneNumber, string? cpf, string? cnpj, Guid? id = null);
}
