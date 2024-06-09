using BusesControl.Entities.Models;
using BusesControl.Entities.Request;

namespace BusesControl.Business.v1.Interfaces;

public interface IUserRegistrationQueueBusiness
{
    Task<bool> GetForCreateAsync(Guid employeeId);
    Task<UserRegistrationQueueModel> GetForRegistrationUserStepCodeAsync(UserRegistrationStepCodeRequest request);
    Task<UserRegistrationQueueModel> GetForRegistrationUserStepPasswordAsync(Guid userId);
    Task<UserRegistrationQueueModel> GetForDeleteAsync(Guid id);
    Task<UserRegistrationQueueModel> GetForApprovedAsync(Guid id);
}
