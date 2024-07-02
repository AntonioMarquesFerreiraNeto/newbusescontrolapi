using BusesControl.Entities.Enums;

namespace BusesControl.Entities.Responses;

public class ContractResponse
{
    public Guid Id { get; set; }
    public Guid BusId { get; set; }
    public BusResponse Bus { get; set; } = default!;
    public Guid DriverId { get; set; }
    public EmployeeResponse Driver { get; set; } = default!;
    public Guid SettingPanelId { get; set; }
    public SettingPanelResponse SettingPanel { get; set; } = default!;
    public ContractDescriptionResponse ContractDescription { get; set; } = default!;
    public decimal TotalPrice { get; set; }
    public ContractPaymentMethodEnum PaymentMethod { get; set; }
    public string? Details { get; set; }
    public int? InstallmentsCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime TerminateDate { get; set; }
    public ContractStatusEnum Status { get; set; }
    public bool IsApproved { get; set; } = false;
    public Guid? ApproverId { get; set; }
    public EmployeeResponse? Approver { get; set; }
    public ICollection<CustomerContractResponse> CustomersContract { get; set; } = default!;
}
