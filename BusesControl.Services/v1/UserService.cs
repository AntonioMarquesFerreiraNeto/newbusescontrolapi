using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Enums;
using BusesControl.Entities.Models;
using BusesControl.Entities.Request;
using BusesControl.Entities.Response;
using BusesControl.Filters.Notification;
using BusesControl.Services.v1.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using PasswordGenerator;
namespace BusesControl.Services.v1;

public class UserService(
    AppSettings _appSettings,
    INotificationApi _notificationApi,
    ITokenService _tokenService,
    IUserBusiness _userBusiness,
    UserManager<UserModel> _userManager
) : IUserService
{
    private static string GeneratedPassword()
    {
        var pwd = new Password(includeLowercase: true, includeUppercase: true, includeNumeric: true, includeSpecial: true, passwordLength: 16);
        var password = pwd.Next();

        return password;
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

        var response = new LoginResponse 
        { 
            Token = token,
            Expires = DateTime.UtcNow.AddHours(_appSettings.JWT.ExpireHours)
        };

        return response;
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

        var password = GeneratedPassword();

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

        //TODO: chamar service de e-mail após implementar a service e integração com recurso de envio de e-mail.

        return true;
    }
}
