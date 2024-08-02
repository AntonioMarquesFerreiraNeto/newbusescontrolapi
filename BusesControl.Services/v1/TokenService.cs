using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Models.v1;
using BusesControl.Filters.Notification;
using BusesControl.Services.v1.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BusesControl.Services.v1;

public class TokenService(
    AppSettings _appSettings,
    INotificationContext _notificationContext,
    UserManager<UserModel> _userManager
) : ITokenService
{
    public async Task<string> GenerateTokenAcess(UserModel user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        if (roles is null || roles.Count == 0)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status401Unauthorized,
                title: NotificationTitle.Unauthorized,
                details: Message.User.CredentialsInvalid
            );
            return default!;
        }

        var userRole = roles.First();

        var claims = new List<Claim>
        {
            new("UserId", user.Id.ToString()),
            new(ClaimTypes.Role, userRole),
            new(ClaimTypes.Email, user.Email!),
        };

        if (user.EmployeeId is not null)
        {
            claims.Add(new("EmployeeId", user.EmployeeId.Value.ToString()));
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.JWT.Key);
        var tokenDescricao = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(_appSettings.JWT.ExpireHours),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescricao);

        return tokenHandler.WriteToken(token);
    }

}
