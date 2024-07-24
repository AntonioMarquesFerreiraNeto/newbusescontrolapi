using BusesControl.Entities.Models;

namespace BusesControl.Persistence.v1.Repositories.Interfaces;

public interface ISupplierRepository
{
    Task<IEnumerable<SupplierModel>> FindBySearchAsync(int page = 0, int pageSize = 0, string? search = null);
    Task<SupplierModel?> GetByIdAsync(Guid id);
    Task<bool> CreateAsync(SupplierModel record);
    bool Update(SupplierModel record);
    Task<bool> ExistsByEmailOrPhoneNumberOrCnpjAsync(string email, string phoneNumber, string cnpj, Guid? id = null);
}
