using BusesControl.Entities.Requests;

namespace BusesControl.Business.v1.Interfaces;

public interface ICustomerBusiness
{
    Task<bool> ExistsByRequestAsync(CustomerCreateRequest request, Guid? id = null);
}
