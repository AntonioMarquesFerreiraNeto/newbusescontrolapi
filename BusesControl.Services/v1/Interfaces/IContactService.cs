using BusesControl.Entities.Requests.v1;

namespace BusesControl.Services.v1.Interfaces;

public interface IContactService
{
    Task<bool> CreateAsync(ContactCreateRequest request);
}
