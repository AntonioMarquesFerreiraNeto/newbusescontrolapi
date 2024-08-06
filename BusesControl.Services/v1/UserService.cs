using AutoMapper;
using BusesControl.Commons;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Enums.v1;
using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Requests.v1;
using BusesControl.Entities.Responses.v1;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using BusesControl.Persistence.UnitOfWork;
using BusesControl.Services.v1.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using PasswordGenerator;
using System.Security.Claims;

namespace BusesControl.Services.v1;

public class UserService(
    AppSettings _appSettings,
    IMapper _mapper,
    IHttpContextAccessor _httpContextAccessor,
    IUnitOfWork _unitOfWork,
    INotificationContext _notificationContext,
    ICacheService _cacheService,
    IEmailService _emailService,
    ITokenService _tokenService,
    IUserRepository _userRepository,
    IResetPasswordSecurityCodeRepository _resetPasswordSecurityCodeRepository,
    UserManager<UserModel> _userManager
) : IUserService
{
    private async Task<string> GenerateUniqueCode()
    {
        var random = new Random();
        var chars = "ABCDEFG0123456789";
        var code = "";

        var existsCode = true;
        while (existsCode)
        {
            for (int c = 0; c < _appSettings.ResetPassword.CodeLength; c++)
            {
                code += chars[random.Next(chars.Length)];
            }

            existsCode = await _resetPasswordSecurityCodeRepository.ExistsByCode(code);
        }

        return code;
    }

    public string GeneratePassword()
    {
        var pwd = new Password(includeLowercase: true, includeUppercase: true, includeNumeric: true, includeSpecial: true, passwordLength: 26);
        var password = pwd.Next();

        return password;
    }

    public async Task<UserResponse> GetByLoggedUserAsync()
    {
        var userId = FindAuthenticatedUser().Id;

        var user = await _cacheService.GetAsync<UserModel>($"user:{userId}");
        if (user is null)
        {
            user = await _userRepository.GetByIdWithEmployeeAsync(userId);
            if (user is null)
            {
                _notificationContext.SetNotification(
                    statusCode: StatusCodes.Status404NotFound,
                    title: NotificationTitle.NotFound,
                    details: Message.User.NotFound
                );
                return default!;
            }

            var cacheOptions = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(_appSettings.Redis.Expire));
            await _cacheService.SetAsync($"user:{userId}", user, cacheOptions);
        }

        return _mapper.Map<UserResponse>(user);
    }


    public UserAuthResponse FindAuthenticatedUser()
    {
        if (_httpContextAccessor.HttpContext == null)
        {
            throw new Exception("HttpContext is not available.");
        }

        var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst("UserId");
        var roleClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role);
        var employeeIdClaim = _httpContextAccessor.HttpContext.User.FindFirst("EmployeeId");

        if (userIdClaim is null || roleClaim is null)
        {
            throw new Exception("User ID or role claims are not present in the token.");
        }

        var id = Guid.Parse(userIdClaim.Value.ToString());
        var role = roleClaim.Value.ToString();
        Guid? employeeId = employeeIdClaim is not null ? Guid.Parse(employeeIdClaim.Value.ToString()) : null;

        return new UserAuthResponse(id, role, employeeId);
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var record = await _userManager.FindByEmailAsync(request.Username);
        if (record is null || record.Status == UserStatusEnum.Inactive)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status401Unauthorized,
                title: NotificationTitle.Unauthorized,
                details: Message.User.CredentialsInvalid
            );
            return default!;
        }

        var result = await _userManager.CheckPasswordAsync(record, request.Password);
        if (result == false)
        {
            _notificationContext.SetNotification(
                 statusCode: StatusCodes.Status401Unauthorized,
                 title: NotificationTitle.Unauthorized,
                 details: Message.User.CredentialsInvalid
            );
            return default!;
        }

        var token = await _tokenService.GenerateTokenAcess(record);
        if (_notificationContext.HasNotification)
        {
            return default!;
        }

        var expires = DateTime.UtcNow.AddHours(_appSettings.JWT.ExpireHours);

        return new LoginResponse(token, expires);
    }

    public async Task<SuccessResponse> ResetPasswordStepCodeAsync(UserResetPasswordStepCodeRequest request)
    {
        request.Cpf = OnlyNumbers.ClearValue(request.Cpf);

        var record = await _userRepository.GetByEmailAndCpfAndBirthDateAsync(request.Email, request.Cpf, request.BirthDate);
        if (record is null)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.User.NotFound
            );
            return default!;
        }

        _unitOfWork.BeginTransaction();

        var resetPasswordCodeRecord = await _resetPasswordSecurityCodeRepository.GetByUserAsync(record.Id);
        if (resetPasswordCodeRecord is not null)
        {
            _resetPasswordSecurityCodeRepository.Remove(resetPasswordCodeRecord);
            await _unitOfWork.CommitAsync();
        }

        var newResetPasswordCodeRecord = new ResetPasswordSecurityCodeModel
        {
            UserId = record.Id,
            Code = await GenerateUniqueCode(),
            Expires = DateTime.UtcNow.AddMinutes(_appSettings.ResetPassword.ExpireCode)
        };

        await _resetPasswordSecurityCodeRepository.AddAsync(newResetPasswordCodeRecord);

        _emailService.SendEmailStepCode(request.Email, record.Employee!.Name, newResetPasswordCodeRecord.Code);
        if (_notificationContext.HasNotification)
        {
            return default!;
        }

        await _unitOfWork.CommitAsync(true);

        return new SuccessResponse(Message.User.SuccessStepOne);
    }

    public async Task<UserResetPasswordStepResetTokenResponse> ResetPasswordStepResetTokenAsync(UserResetPasswordStepResetTokenRequest request)
    {
        var resetPasswordRecord = await _resetPasswordSecurityCodeRepository.GetByCodeAsync(request.Code);
        if (resetPasswordRecord is null)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.ResetUser.UnexpectedToken
            );
            return default!;
        }

        var difference = DateTime.UtcNow - resetPasswordRecord.Expires;
        var expireCode = TimeSpan.FromMinutes(_appSettings.ResetPassword.ExpireCode);
        if (difference >= expireCode)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.ResetUser.CodeInvalid
            );
            return default!;
        }

        var userRecord = await _userManager.FindByIdAsync(resetPasswordRecord.UserId.ToString());
        if (userRecord is null)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.User.NotFound
            );
            return default!;
        }

        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(userRecord);
        if (resetToken is null)
        {
            _notificationContext.SetNotification(
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
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.User.NotFound
            );
            return default!;
        }

        if (request.NewPassword != request.ConfirmPassword)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.ResetUser.InvalidPassword
            );
            return default!;
        }

        var result = await _userManager.ResetPasswordAsync(record, request.ResetToken, request.NewPassword);
        if (!result.Succeeded)
        {
            _notificationContext.SetNotification(
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
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.ResetUser.InvalidPassword
            );
            return default!;
        }

        if (request.NewPassword == request.CurrentPassword)
        {
            _notificationContext.SetNotification(
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
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.User.NotFound
            );
            return default!;
        }

        var result = await _userManager.CheckPasswordAsync(userRecord, request.CurrentPassword);
        if (!result)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.User.InvalidCurrentPassword
            );
            return default!;
        }

        var changePasswordResult = await _userManager.ChangePasswordAsync(userRecord, request.CurrentPassword, request.NewPassword);
        if (!changePasswordResult.Succeeded)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status500InternalServerError,
                title: NotificationTitle.InternalError,
                details: Message.User.Unexpected
            );
            return default!;
        }

        return new SuccessResponse(Message.User.SuccessChangePassword);
    }

    public async Task<UserModel> CreateForUserRegistrationAsync(UserCreateRequest request)
    {
        var userExists = await _userManager.FindByEmailAsync(request.Email);
        if (userExists is not null)
        {
            return default!;
        }

        if (request.Role == "Driver")
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.User.InvalidRole
            );
            return default!;
        }

        var record = new UserModel
        {
            EmployeeId = request.EmployeeId,
            NickName = request.Name.Split(' ')[0],
            UserName = request.Email,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber
        };

        var passwordDefault = GeneratePassword();

        var result = await _userManager.CreateAsync(record, passwordDefault);
        if (!result.Succeeded)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status500InternalServerError,
                title: NotificationTitle.InternalError,
                details: Message.User.Unexpected
            );
            return default!;
        }

        var role = await _userManager.AddToRoleAsync(record, request.Role);
        if (!role.Succeeded)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status500InternalServerError,
                title: NotificationTitle.InternalError,
                details: Message.User.Unexpected
            );
            return default!;
        }

        return record;
    }

    public async Task<bool> SetNicknameAsync(UserSetNickNameRequest request)
    {
        var userId = FindAuthenticatedUser().Id.ToString();

        var record = await _userManager.FindByIdAsync(userId);
        if (record is null)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.User.NotFound
            );
            return false;
        }

        record.NickName = request.NickName;

        var result = await _userManager.UpdateAsync(record);
        if (!result.Succeeded)
        {
            _notificationContext.SetNotification(
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
            _notificationContext.SetNotification(
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
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status500InternalServerError,
                title: NotificationTitle.InternalError,
                details: Message.User.Unexpected
            );
            return false;
        }

        return true;
    }

    public async Task<bool> ActiveForAprrovedUserRegistrationAsync(Guid id)
    {
        var record = await _userManager.FindByIdAsync(id.ToString());
        if (record is null)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.User.NotFound
            );
            return false;
        }

        record.Status = UserStatusEnum.Active;
        var result = await _userManager.UpdateAsync(record);
        if (!result.Succeeded)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status500InternalServerError,
                title: NotificationTitle.InternalError,
                details: Message.User.Unexpected
            );
            return false;
        }

        return true;
    }

    public async Task<bool> DeleteForUserRegistrationAsync(Guid id)
    {
        var record = await _userManager.FindByIdAsync(id.ToString());
        if (record is null)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.User.NotFound
            );
            return false;
        }

        var result = await _userManager.DeleteAsync(record);
        if (!result.Succeeded)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status500InternalServerError,
                title: NotificationTitle.InternalError,
                details: Message.User.Unexpected
            );
            return false;
        }

        return true;
    }

    public async Task<SuccessResponse> ToggleRoleForEmployeeAsync(string email, EmployeeTypeEnum employeeType)
    {
        if (employeeType == EmployeeTypeEnum.Driver)
        {
            return new SuccessResponse(Message.Employee.RoleChangedNoAccess);
        }

        var user = await _userManager.FindByEmailAsync(email);
        if (user is null) 
        {
            return new SuccessResponse(Message.Employee.RoleChangedRegisterInQueue);
        }

        var roles = await _userManager.GetRolesAsync(user);
        if (roles is null || roles.Count == 0)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status500InternalServerError,
                title: NotificationTitle.InternalError,
                details: Message.User.Unexpected
            );
            return default!;
        }

        var oldRole = roles.First();
        var newRole = employeeType.ToString();

        var roleRemoveResult = await _userManager.RemoveFromRoleAsync(user, oldRole);
        if (!roleRemoveResult.Succeeded)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status500InternalServerError,
                title: NotificationTitle.InternalError,
                details: Message.User.Unexpected
            );
            return default!;
        }

        var roleAddResult = await _userManager.AddToRoleAsync(user, newRole);
        if (!roleAddResult.Succeeded)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status500InternalServerError,
                title: NotificationTitle.InternalError,
                details: Message.User.Unexpected
            );
            return default!;
        }

        return new SuccessResponse(Message.Employee.RoleAndProfileChanged);
    }

    public async Task<bool> UpdateForEmployeeAsync(UserUpdateRequest request)
    {        
        var userRecord = await _userManager.FindByEmailAsync(request.OldEmail);
        if (userRecord is null)
        {
            return false;
        }

        userRecord.UserName = request.NewEmail;
        userRecord.Email = request.NewEmail;
        userRecord.PhoneNumber = request.NewPhoneNumber;

        var result = await _userManager.UpdateAsync(userRecord);
        if (!result.Succeeded)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.User.Unexpected
            );
            return false;
        }

        return true;
    }
}
