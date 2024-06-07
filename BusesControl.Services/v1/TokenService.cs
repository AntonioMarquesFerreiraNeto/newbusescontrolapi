using BusesControl.Commons;
using BusesControl.Entities.Models;
using BusesControl.Services.v1.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BusesControl.Services.v1;

public class TokenService(
    AppSettings _appSettings
) : ITokenService
{
    public string GeneratedTokenAcess(UserModel user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.JWT.Key);
        var tokenDescricao = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] {
                    new Claim("UserId", user.Id),
                    new Claim("EmployeeId", user.EmployeeId.ToString()),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim(ClaimTypes.Email, user.Email!)
            }),
            Expires = DateTime.UtcNow.AddHours(_appSettings.JWT.ExpireHours),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescricao);

        return tokenHandler.WriteToken(token);
    }
}
