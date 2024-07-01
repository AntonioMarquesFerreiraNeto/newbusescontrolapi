using System.ComponentModel.DataAnnotations;

namespace BusesControl.Entities.Models;

public class ContractDescriptionModel
{
    [Key]
    public Guid Id { get; set; }
    [MaxLength(8)]
    public string Reference { get; set; } = default!;
    [MaxLength(3000)]
    public string Owner { get; set; } = default!;
    [MaxLength(3000)]
    public string GeneralProvisions { get; set; } = default!;
    [MaxLength(3000)]
    public string Objective { get; set; } = default!;
    [MaxLength(70)]
    public string Title { get; set; } = default!;
    [MaxLength(70)]
    public string SubTitle { get; set; } = default!;
    [MaxLength(70)]
    public string Copyright { get; set; } = default!;
}
