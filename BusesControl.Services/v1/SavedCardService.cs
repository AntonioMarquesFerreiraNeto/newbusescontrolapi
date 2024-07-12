using BusesControl.Entities.Models;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using BusesControl.Persistence.v1.UnitOfWork;
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
        await _savedCardRepository.CreateAsync(record);
        await _unitOfWork.CommitAsync();

        return true;
    }
}
