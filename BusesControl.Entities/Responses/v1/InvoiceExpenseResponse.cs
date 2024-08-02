using BusesControl.Entities.Enums.v1;

namespace BusesControl.Entities.Responses.v1;

public class InvoiceExpenseResponse
{
    public Guid Id { get; set; }
    public string Reference { get; set; } = default!;
    public Guid FinancialId { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal TotalPrice { get; set; }
    public decimal Price { get; set; }
    public decimal InterestRate { get; set; }
    public InvoiceStatusEnum Status { get; set; }
    public DateOnly DueDate { get; set; }
    public string? ExternalId { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public DateOnly? PaymentDate { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
