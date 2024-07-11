using BusesControl.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BusesControl.Entities.Models;

public class ContractModel
{
    public Guid Id { get; set; }
    [MaxLength(8)]
    public string Reference { get; set; } = default!;
    public Guid BusId { get; set; }
    public BusModel Bus { get; set; } = default!;
    public Guid DriverId { get; set; }
    public EmployeeModel Driver { get; set; } = default!;
    public Guid SettingPanelId { get; set; }
    public SettingPanelModel SettingPanel { get; set; } = default!;
    public Guid ContractDescriptionId { get; set; }
    public ContractDescriptionModel ContractDescription { get; set; } = default!;
    [Precision(10, 2)]
    public decimal TotalPrice { get; set; }
    public PaymentTypeEnum PaymentType { get; set; }
    [MaxLength(2500)]
    public string? Details { get; set; }
    public int? InstallmentsCount { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly TerminateDate { get; set; }
    public ContractStatusEnum Status { get; set; } = ContractStatusEnum.WaitingReview;
    public bool IsApproved { get; set; } = false;
    public Guid? ApproverId { get; set; }
    public EmployeeModel? Approver { get; set; } = default!;
    public int CustomersCount { get; set; }
    public ICollection<CustomerContractModel> CustomersContract { get; set; } = default!;
}
