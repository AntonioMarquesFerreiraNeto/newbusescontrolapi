using BusesControl.Entities.Enums;

namespace BusesControl.Entities.DTOs;

public class CreateInvoiceDTO
{
    public Guid FinancialId { get; set; }
    public string ExternalId { get; set; } = default!;
    public string ContractReference { get; set; } = default!;
    public int Index { get; set; }
    public decimal Price { get; set; }
    public DateOnly DueDate { get; set; }
    public PaymentTypeEnum PaymentType { get; set; }
}
