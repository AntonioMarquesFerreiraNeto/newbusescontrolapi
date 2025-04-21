using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Requests.v1;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using BusesControl.Persistence.UnitOfWork;
using BusesControl.Services.v1.Interfaces;

namespace BusesControl.Services.v1;

public class ContactService(
    IContactRepository _contactRepository,
    IUnitOfWork _unitOfWork
) : IContactService
{
    public async Task<bool> CreateAsync(ContactCreateRequest request)
    {
        var record = new ContactModel 
        { 
            Name = request.Name,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Message = request.Message,
            CreatedAt = DateTime.Now
        };

        await _contactRepository.AddAsync(record);
        await _unitOfWork.CommitAsync();

        return true;
    }
}
