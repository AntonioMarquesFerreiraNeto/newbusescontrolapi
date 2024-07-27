namespace BusesControl.Entities.DTOs;

public class CreateInvoiceExpenseInDTO
{
    public int Index { get; set; }
    public Guid FinancialId { get; set; }
    public string FinancialReference { get; set; } = default!;
    public decimal Price { get; set; }
    public DateOnly DueDate { get; set; }
}
