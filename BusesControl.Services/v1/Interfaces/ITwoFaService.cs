using BusesControl.Entities.Requests.v1;
using BusesControl.Entities.Responses.v1;

namespace BusesControl.Services.v1.Interfaces
{
    public interface ITwoFaService
    {
        Task<bool> CreateAsync(string ipLocation, CreateTwoRequest request);
        Task<TwoFaValidateCodeResponse> ValidateCodeAsync(TwoFaValidateCodeRequest request);
        Task<bool> ValidateTokenAsync(TwoFaValidateTokenRequest request);
        Task<bool> CheckForNew(string ipLocation, TwoFaCheckForNewRequest request);
    }
}
