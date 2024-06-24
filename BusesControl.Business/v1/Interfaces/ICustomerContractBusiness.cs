namespace BusesControl.Business.v1.Interfaces;

public interface ICustomerContractBusiness
{
    Task<bool> ValidateForCreateAsync(Guid customerId);
}
