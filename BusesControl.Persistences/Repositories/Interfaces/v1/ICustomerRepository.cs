using BusesControl.Entities.Models.v1;

namespace BusesControl.Persistence.Repositories.Interfaces.v1;

public interface ICustomerRepository : IGenericRepository<CustomerModel>
{
    Task<IEnumerable<CustomerModel>> FindBySearchAsync(int page = 0, int pageSize = 0, string? search = null);
    Task<int> CountBySearchAsync(string? search = null);
    Task<CustomerModel?> GetByIdAsync(Guid id);
    Task<CustomerModel?> GetByExternalAsync(string externalId);
    Task<bool> ExistsByEmailOrPhoneNumberOrCpfOrCnpjAsync(string email, string phoneNumber, string? cpf, string? cnpj, Guid? id = null);
}
