using BusesControl.Entities.Responses.v1;

namespace BusesControl.Services.v1.Interfaces;

public interface IViaCepIntegrationService
{
    Task<AddressResponse> GetAddressByCepAsync(string cep);
}
