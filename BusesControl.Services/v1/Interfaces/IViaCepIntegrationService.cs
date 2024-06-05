using BusesControl.Entities.Response;

namespace BusesControl.Services.v1.Interfaces;

public interface IViaCepIntegrationService
{
    Task<AddressResponse> GetAddressByCepAsync(string cep);
}
