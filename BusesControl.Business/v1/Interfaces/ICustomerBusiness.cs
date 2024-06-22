namespace BusesControl.Business.v1.Interfaces;

public interface ICustomerBusiness
{
    Task<bool> ExistsByEmailOrPhoneNumberOrCpfOrCnpjAsync(string email, string phoneNumber, string? cpf, string? cnpj, Guid? id = null);
}
