using BusesControl.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BusesControl.Entities.Models;

public class InvoiceModel
{
    public Guid Id { get; set; }
    [MaxLength(8)]
    public string Reference { get; set; } = default!;
    public Guid FinancialId { get; set; }
    public FinancialModel Financial { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    [Precision(10, 2)]
    public decimal TotalPrice { get; set; }
    [Precision(10, 2)]
    public decimal Price { get; set; }
    [Precision(10, 2)]
    public decimal InterestRate { get; set; }
    public InvoiceStatusEnum Status { get; set; } = InvoiceStatusEnum.Pending;
    public DateOnly DueDate { get; set; }
    public string? ExternalId { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
