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
    Task<bool> CreateForEmployeeAsync(UserCreateRequest request);
    Task<bool> SetNicknameAsync(UserSetNickNameRequest request);
    Task<bool> ToggleActiveAsync(Guid id, UserToggleActiveRequest request);
}
