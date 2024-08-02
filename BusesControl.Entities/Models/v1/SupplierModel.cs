using System.ComponentModel.DataAnnotations;

namespace BusesControl.Entities.Models.v1;

public class SupplierModel
{
    public Guid Id { get; set; }
    [MaxLength(90)]
    public string Name { get; set; } = default!;
    [MaxLength(14)]
    public string Cnpj { get; set; } = default!;
    public DateOnly OpenDate { get; set; } = default!;
    [MaxLength(80)]
    public string Email { get; set; } = default!;
    [MaxLength(11)]
    public string PhoneNumber { get; set; } = default!;
    [MaxLength(20)]
    public string HomeNumber { get; set; } = default!;
    [MaxLength(60)]
    public string Logradouro { get; set; } = default!;
    [MaxLength(60)]
    public string ComplementResidential { get; set; } = default!;
    [MaxLength(60)]
    public string Neighborhood { get; set; } = default!;
    [MaxLength(60)]
    public string City { get; set; } = default!;
    [MaxLength(60)]
    public string State { get; set; } = default!;
    public bool Active { get; set; } = true;
}
