using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Requests.v1;

namespace BusesControl.Business.v1.Interfaces;

public interface IUserRegistrationQueueBusiness
{
    Task<EmployeeModel> GetForValidateForCreateAsync(Guid employeeId);
    Task<UserRegistrationQueueModel> GetForRegistrationUserStepCodeAsync(UserRegistrationStepCodeRequest request);
    Task<UserRegistrationQueueModel> GetForRegistrationUserStepPasswordAsync(Guid userId);
    Task<UserRegistrationQueueModel> GetForDeleteAsync(Guid id);
    Task<UserRegistrationQueueModel> GetForApproveAsync(Guid id);
}
