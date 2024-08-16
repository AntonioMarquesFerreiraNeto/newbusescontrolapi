using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Requests.v1;
using BusesControl.Entities.Responses.v1;

namespace BusesControl.Services.v1.Interfaces;

public interface IUserRegistrationQueueService
{
    Task<PaginationResponse<UserRegistrationQueueModel>> FindBySearchAsync(PaginationRequest request);
    Task<SuccessResponse> CreateForEmployeeAsync(UserRegistrationCreateRequest request);
    Task<SuccessResponse> RegistrationUserStepCodeAsync(UserRegistrationStepCodeRequest request);
    Task<UserRegistrationStepTokenResponse> RegistrationUserStepTokenAsync(UserRegistrationStepTokenRequest request);
    Task<SuccessResponse> RegistrationUserStepPasswordAsync(UserRegistrationStepPasswordRequest request);
    Task<SuccessResponse> DeleteAsync(Guid id);
    Task<SuccessResponse> AprroveAsync(Guid id);
}
