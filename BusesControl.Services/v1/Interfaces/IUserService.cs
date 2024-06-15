using BusesControl.Entities.Enums;
using BusesControl.Entities.Models;
using BusesControl.Entities.Request;
using BusesControl.Entities.Response;

namespace BusesControl.Services.v1.Interfaces;

public interface IUserService
{
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
}
