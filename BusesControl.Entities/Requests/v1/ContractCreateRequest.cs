using BusesControl.Entities.Enums.v1;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BusesControl.Entities.Requests.v1;

public class ContractCreateRequest
{
    public Guid BusId { get; set; }
    public Guid DriverId { get; set; }
    public Guid SettingPanelId { get; set; }
    public Guid ContractDescriptionId { get; set; }
    [Precision(10, 2)]
    public decimal TotalPrice { get; set; }
    public PaymentTypeEnum PaymentType { get; set; }
    [MaxLength(2500)]
    public string? Details { get; set; }
    public DateOnly TerminateDate { get; set; }
    public IEnumerable<Guid> CustomersId { get; set; } = default!;
}
