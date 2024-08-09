using BusesControl.Entities.Models.v1;

namespace BusesControl.Persistence.Repositories.Interfaces.v1;

public interface ISupplierRepository : IGenericRepository<SupplierModel>
{
    Task<IEnumerable<SupplierModel>> FindBySearchAsync(int page = 0, int pageSize = 0, string? search = null);
    Task<int> CountBySearchAsync(string? search = null);
    Task<SupplierModel?> GetByIdAsync(Guid id);
    Task<bool> ExistsByEmailOrPhoneNumberOrCnpjAsync(string email, string phoneNumber, string cnpj, Guid? id = null);
}
