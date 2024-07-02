using BusesControl.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BusesControl.Entities.Models;

public class SettingPanelModel
{
    public Guid Id { get; set; }
    [MaxLength(8)]
    public string Reference { get; set; } = default!;
    [Precision(5, 2)]
    public decimal TerminationFee { get; set; }
    [Precision(5, 2)]
    public decimal LateFeeInterestRate { get; set; }
    public bool CustomerDelinquencyEnabled { get; set; }
    public int? LimitDateTermination { get; set; }
    public SettingPanelParentEnum Parent { get; set; }
    public bool Active { get; set; }
    public Guid RequesterId { get; set; }
    public EmployeeModel Requester { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}