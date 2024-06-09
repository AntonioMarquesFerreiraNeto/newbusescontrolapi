using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Enums;
using BusesControl.Entities.Models;
using BusesControl.Entities.Request;
using BusesControl.Entities.Response;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using BusesControl.Persistence.v1.UnitOfWork;
using BusesControl.Services.v1.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using PasswordGenerator;
using System.Security.Claims;
namespace BusesControl.Services.v1;

public class UserService(
    IHttpContextAccessor _httpContextAccessor,
    IUnitOfWork _unitOfWork,
    INotificationApi _notificationApi,
    ITokenService _tokenService,
    IUserRepository _userRepository,
    IResetUserRepository _resetUserRepository,
    IUserBusiness _userBusiness,
    UserManager<UserModel> _userManager
) : IUserService
{
    private static string GeneratePassword()
    {
        var pwd = new Password(includeLowercase: true, includeUppercase: true, includeNumeric: true, includeSpecial: true, passwordLength: 16);
        var password = pwd.Next();

        return password;
    }

    private async Task<string> GenerateUniqueCode()
    {
        var random = new Random();
        var chars = "abcdefghijklnopqyz0123456789";
        var code = "";

        var existsCode = true;
        while (existsCode)
        {
            for (int c = 0; c < 10; c++)
            {
                var caracter = chars[random.Next(chars.Length)];
                code += caracter;
            }

            existsCode = await _resetUserRepository.ExistsByCode(code);
        }

        return code;
    }

    public UserAuthResponse FindAuthenticatedUser()
    {
        if (_httpContextAccessor.HttpContext == null)
        {
            throw new Exception("HttpContext is not available.");
        }

        var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst("UserId");
        var roleClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role);

        if (userIdClaim is null || roleClaim is null)
        {
            throw new Exception("User ID or role claims are not present in the token.");
        }

        var id = Guid.Parse(userIdClaim.Value.ToString());
        var role = roleClaim.ToString();

        return new UserAuthResponse(id, role);
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var record = await _userManager.FindByEmailAsync(request.Username);
        if (record is null || record.Status == UserStatusEnum.Inactive)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status401Unauthorized,
                title: NotificationTitle.Unauthorized,
                details: Message.User.CredentialsInvalid
            );
            return default!;
        }

        var result = await _userManager.CheckPasswordAsync(record, request.Password);
        if (result == false)
        {
            _notificationApi.SetNotification(
                 statusCode: StatusCodes.Status401Unauthorized,
                 title: NotificationTitle.Unauthorized,
                 details: Message.User.CredentialsInvalid
            );
            return default!;
        }

        var token = _tokenService.GeneratedTokenAcess(record);
        var expires = DateTime.UtcNow.AddHours(AppSettingsJWT.ExpireHours);

        return new LoginResponse(token, expires);
    }

    public async Task<SuccessResponse> ResetPasswordStepCodeAsync(UserResetPasswordStepCodeRequest request)
    {
        request.Cpf = OnlyNumbers.ClearValue(request.Cpf);

        var record = await _userRepository.GetByEmailAndCpfAndBirthDateAsync(request.Email, request.Cpf, request.BirthDate);
        if (record is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.User.NotFound
            );
            return default!;
        }

        _unitOfWork.BeginTransaction();

        var oldResetUserRecord = await _resetUserRepository.GetByUserAsync(record.Id);
        if (oldResetUserRecord is not null)
        {
            _resetUserRepository.Remove(oldResetUserRecord);
            await _unitOfWork.CommitAsync();
        }

        var newResetUserRecord = new ResetUserModel
        {
            UserId = record.Id,
            Code = await GenerateUniqueCode(),
            Expires = DateTime.UtcNow.AddMinutes(AppSettingsResetPassword.ExpireCode)
        };
        await _resetUserRepository.Create(newResetUserRecord);

        //TODO: Enviar código gerado para o e-mail do usuário neste local.

        await _unitOfWork.CommitAsync(true);

        return new SuccessResponse(Message.User.SuccessStepOne);
    }

    public async Task<UserResetPasswordStepResetTokenResponse> ResetPasswordStepResetTokenAsync(UserResetPasswordStepResetTokenRequest request)
    {
        var resetUserRecord = await _resetUserRepository.GetByCodeAsync(request.Code);
        if (resetUserRecord is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.ResetUser.UnexpectedToken
            );
            return default!;
        }

        var difference = DateTime.UtcNow - resetUserRecord.Expires;
        var expireCode = TimeSpan.FromMinutes(AppSettingsResetPassword.ExpireCode);
        if (difference >= expireCode)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.ResetUser.CodeInvalid
            );
            return default!;
        }

        var userRecord = await _userManager.FindByIdAsync(resetUserRecord.UserId.ToString());
        if (userRecord is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.User.NotFound
            );
            return default!;
        }

        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(userRecord);
        if (resetToken is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.ResetUser.UnexpectedToken
            );
            return default!;
        }

        return new UserResetPasswordStepResetTokenResponse(userRecord.Id, resetToken);
    }

    public async Task<SuccessResponse> ResetPasswordStepNewPasswordAsync(UserResetPasswordStepNewPasswordRequest request)
    {
        var record = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (record is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.User.NotFound
            );
            return default!;
        }

        if (request.NewPassword != request.ConfirmPassword)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.ResetUser.InvalidPassword
            );
            return default!;
        }

        var result = await _userManager.ResetPasswordAsync(record, request.ResetToken, request.NewPassword);
        if (!result.Succeeded)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status500InternalServerError,
                title: NotificationTitle.InternalError,
                details: Message.User.Unexpected
            );
            return default!;
        }

        return new SuccessResponse(Message.ResetUser.Success);
    }

    public async Task<SuccessResponse> ChangePasswordAsync(UserChangePasswordRequest request)
    {
        if (request.NewPassword != request.ConfirmPassword)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.ResetUser.InvalidPassword
            );
            return default!;
        }

        if (request.NewPassword == request.CurrentPassword)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.User.InvalidNewPassword
            );
            return default!;
        }

        var userId = FindAuthenticatedUser().Id.ToString();
        
        var userRecord = await _userManager.FindByIdAsync(userId);
        if (userRecord is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.User.NotFound
            );
            return default!;
        }

        var result = await _userManager.CheckPasswordAsync(userRecord, request.CurrentPassword);
        if (!result)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.User.InvalidCurrentPassword
            );
            return default!;
        }

        var changePasswordResult = await _userManager.ChangePasswordAsync(userRecord, request.CurrentPassword, request.NewPassword);
        if (!changePasswordResult.Succeeded)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status500InternalServerError,
                title: NotificationTitle.InternalError,
                details: Message.User.Unexpected
            );
            return default!;
        }

        return new SuccessResponse(Message.User.SuccessChangePassword);
    }

    public async Task<bool> CreateForEmployeeAsync(UserCreateRequest request)
    {
        await _userBusiness.ValidateForCreateAsync(request.Email, request.Role);
        if (_notificationApi.HasNotification)
        {
            return false;
        }

        var record = new UserModel
        {
            EmployeeId = request.EmployeeId,
            UserName = request.Email,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Role = request.Role
        };

        var password = GeneratePassword();

        var result = await _userManager.CreateAsync(record, password);
        if (!result.Succeeded)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status500InternalServerError,
                title: NotificationTitle.InternalError,
                details: Message.User.Unexpected
            );
            return false;
        }

        //TODO: enviar e-mail para o usuário informando sobre os passos de autenticação para ele.

        return true;
    }

    public async Task<bool> SetNicknameAsync(UserSetNickNameRequest request)
    {
        var userId = FindAuthenticatedUser().Id.ToString();

        var record = await _userManager.FindByIdAsync(userId);
        if (record is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.User.NotFound
            );
            return false;
        }

        record.Nickname = request.Nickname;

        var result = await _userManager.UpdateAsync(record);
        if (!result.Succeeded)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status500InternalServerError,
                title: NotificationTitle.InternalError,
                details: Message.User.Unexpected
            );
            return false;
        }

        return true;
    }

    public async Task<bool> ToggleActiveAsync(Guid id, UserToggleActiveRequest request)
    {
        var record = await _userManager.FindByIdAsync(id.ToString());
        if (record is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.User.NotFound
            );
            return false;
        }

        record.Status = request.Status;

        var result = await _userManager.UpdateAsync(record);
        if (!result.Succeeded)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status500InternalServerError,
                title: NotificationTitle.InternalError,
                details: Message.User.Unexpected
            );
            return false;
        }

        return true;
    }
}
