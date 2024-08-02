namespace BusesControl.Entities.Models.v1;

public class SavedCardModel
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public CustomerModel Customer { get; set; } = default!;
    public string CreditCardNumber { get; set; } = default!;
    public string CreditCardBrand { get; set; } = default!;
    public Guid CreditCardToken { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
