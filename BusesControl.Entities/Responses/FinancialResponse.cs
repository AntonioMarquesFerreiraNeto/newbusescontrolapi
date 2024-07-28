using BusesControl.Entities.Enums;

namespace BusesControl.Entities.Responses;

public class FinancialResponse
{
    public Guid Id { get; set; }
    public string Reference { get; set; } = default!;
    public Guid? ContractId { get; set; }
    public Guid? SettingPanelId { get; set; }
    public SettingPanelResponse? SettingPanel { get; set; }
    public Guid? CustomerId { get; set; }
    public CustomerResponse? Customer { get; set; } = default!;
    public Guid? SupplierId { get; set; }
    public SupplierResponse? Supplier { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal TotalPrice { get; set; }
    public decimal Price { get; set; }
    public decimal TotalInterestRate { get; set; }
    public int InstallmentsCount { get; set; }
    public bool Active { get; set; } = true;
    public FinancialTypeEnum Type { get; set; }
    public PaymentTypeEnum PaymentType { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly TerminateDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public ICollection<InvoiceResponse> Invoices { get; set; } = default!;
    public ICollection<InvoiceExpenseResponse> InvoiceExpenses { get; set; } = default!;
}
