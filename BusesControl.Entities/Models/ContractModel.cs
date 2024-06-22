using BusesControl.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BusesControl.Entities.Models;

public class ContractModel
{
    public Guid Id { get; set; }
    public Guid BusId { get; set; }
    public BusModel Bus { get; set; } = default!;
    public Guid DriverId { get; set; }
    public EmployeeModel Driver { get; set; } = default!;
    [Precision(10, 2)]
    public decimal TotalPrice { get; set; }
    public ContractPaymentMethodEnum PaymentMethod { get; set; }
    [MaxLength(2500)]
    public string? Details { get; set; }
    public int? InstallmentsCount { get; set; }
    public DateTime CreatedAt { get; set; } = default!;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime TerminateDate { get; set; }
    public ContractStatusEnum Status { get; set; } = ContractStatusEnum.WaitingReview;
    public bool IsApproved { get; set; } = false;
    public Guid? ApproverId { get; set; }
    public EmployeeModel? Approver { get; set; } = default!;
    public virtual ICollection<CustomerContractModel> CustomersContract { get; set; } = default!;
}
