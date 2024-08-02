using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Requests.v1;

namespace BusesControl.Business.v1.Interfaces;

public interface IEmployeeBusiness
{
    Task<bool> ExistsByEmailOrPhoneNumberOrCpfAsync(string email, string phoneNumber, string cpf, Guid? id = null);
    Task<EmployeeModel> GetForToggleTypeAsync(Guid id, EmployeeToggleTypeRequest request);
    Task<bool> ValidateForContractDriverReplacementAsync(Guid id);
}
