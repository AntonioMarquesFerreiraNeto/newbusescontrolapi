namespace BusesControl.Business.v1.Interfaces;

public interface IEmployeeBusiness
{
    Task<bool> ExistsByEmailOrPhoneNumberOrCpfAsync(string email, string phoneNumber, string cpf, Guid? id = null);
}
