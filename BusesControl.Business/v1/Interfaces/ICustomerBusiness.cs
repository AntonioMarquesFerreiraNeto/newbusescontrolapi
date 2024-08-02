using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Requests.v1;

namespace BusesControl.Business.v1.Interfaces;

public interface ICustomerBusiness
{
    Task<bool> ExistsByRequestAsync(CustomerCreateRequest request, Guid? id = null);
    Task<CustomerModel> GetWithValidateActiveAsync(Guid id);
}
