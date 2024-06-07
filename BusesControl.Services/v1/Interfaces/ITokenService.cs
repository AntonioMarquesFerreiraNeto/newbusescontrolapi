using BusesControl.Entities.Models;

namespace BusesControl.Services.v1.Interfaces;

public interface ITokenService
{
    string GeneratedTokenAcess(UserModel user);
}
