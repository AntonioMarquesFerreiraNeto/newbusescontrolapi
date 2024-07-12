namespace BusesControl.Services.v1.Interfaces;

public interface ISavedCardService
{
    Task<bool> CreateAsync(Guid customerId, string creditCardNumber, string creditCardBrand, Guid creditCardToken);
}
