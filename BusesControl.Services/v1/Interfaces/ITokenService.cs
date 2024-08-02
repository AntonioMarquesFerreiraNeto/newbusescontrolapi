using BusesControl.Entities.Models.v1;

namespace BusesControl.Services.v1.Interfaces;

public interface ITokenService
{
    Task<string> GenerateTokenAcess(UserModel user);
}
