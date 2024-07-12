namespace BusesControl.Entities.DTOs;

public class InvoicePayWithCardInAssasDTO
{
    public string Status { get; set; } = default!;
    public DateOnly ConfirmedDate { get; set; } = default!;
    public CreditCardInAssasDTO CreditCard { get; set;} = default!;
}
