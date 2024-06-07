using BusesControl.Entities.Request;
using BusesControl.Entities.Response;

namespace BusesControl.Services.v1.Interfaces;

public interface IUserService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task<bool> CreateForEmployeeAsync(UserCreateRequest request);
}
