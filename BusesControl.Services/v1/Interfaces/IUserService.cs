using BusesControl.Entities.Enums.v1;
using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Requests.v1;
using BusesControl.Entities.Responses.v1;

namespace BusesControl.Services.v1.Interfaces;

public interface IUserService
{
    string GeneratePassword();
    Task<PaginationResponse<UserResponse>> FindBySearchForAdminAsync(PaginationRequest request);
    Task<UserResponse> GetByLoggedUserAsync();
    UserAuthResponse FindAuthenticatedUser();
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task<SuccessResponse> ResetPasswordStepCodeAsync(UserResetPasswordStepCodeRequest request);
    Task<UserResetPasswordStepResetTokenResponse> ResetPasswordStepResetTokenAsync(UserResetPasswordStepResetTokenRequest request);
    Task<SuccessResponse> ResetPasswordStepNewPasswordAsync(UserResetPasswordStepNewPasswordRequest request);
    Task<SuccessResponse> ChangePasswordAsync(UserChangePasswordRequest request);
    Task<UserModel> CreateForUserRegistrationAsync(UserCreateRequest request);
    Task<bool> SetNicknameAsync(UserSetNickNameRequest request);
    Task<bool> ToggleActiveAsync(Guid id, UserToggleActiveRequest request);
    Task<bool> DeleteForUserRegistrationAsync(Guid id);
    Task<bool> ActiveForAprrovedUserRegistrationAsync(Guid id);
    Task<SuccessResponse> ToggleRoleForEmployeeAsync(string email, EmployeeTypeEnum employeeType);
    Task<bool> UpdateForEmployeeAsync(UserUpdateRequest request);
}
