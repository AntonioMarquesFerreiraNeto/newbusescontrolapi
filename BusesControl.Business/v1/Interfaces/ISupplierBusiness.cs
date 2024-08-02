using BusesControl.Entities.Models.v1;

namespace BusesControl.Business.v1.Interfaces;

public interface ISupplierBusiness
{
    Task<bool> ExistsByEmailOrPhoneNumberOrCnpjAsync(string email, string phoneNumber, string cnpj, Guid? id = null);
    Task<SupplierModel> GetWithValidateActiveAsync(Guid id);
}
