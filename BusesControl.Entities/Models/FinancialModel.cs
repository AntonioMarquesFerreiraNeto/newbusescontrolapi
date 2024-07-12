using BusesControl.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Entities.Models;

public class FinancialModel
{
    public Guid Id { get; set; }
    public Guid? ContractId { get; set; }
    public ContractModel? Contract { get; set; }
    public Guid? CustomerId { get; set; }
    public CustomerModel? Customer { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    [Precision(10, 2)]
    public decimal TotalPrice { get; set; }
    [Precision(10, 2)]
    public decimal Price { get; set; }
    [Precision(10, 2)]
    public decimal TotalInterestRate { get; set; }
    public int InstallmentsCount { get; set; }
    public bool Active { get; set; } = true;
    public FinancialTypeEnum Type { get; set; }
    public PaymentTypeEnum PaymentType { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly TerminateDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public ICollection<InvoiceModel> Invoices { get; set; } = default!;
}
