using BusesControl.Entities.Models.v1;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using BusesControl.Persistence.UnitOfWork;
using BusesControl.Services.v1.Interfaces;

namespace BusesControl.Services.v1;

public class SavedCardService(
    IUnitOfWork _unitOfWork,
    ISavedCardRepository _savedCardRepository
) : ISavedCardService
{
    public async Task<bool> CreateAsync(Guid customerId, string creditCardNumber, string creditCardBrand, Guid creditCardToken)
    {
        var record = new SavedCardModel
        {
            CustomerId = customerId,
            CreditCardNumber = creditCardNumber,
            CreditCardBrand = creditCardBrand,
            CreditCardToken = creditCardToken
        };
        await _savedCardRepository.AddAsync(record);
        await _unitOfWork.CommitAsync();

        return true;
    }
}
