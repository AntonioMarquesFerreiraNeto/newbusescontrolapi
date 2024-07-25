using BusesControl.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace BusesControl.Entities.Models;

public class EmployeeModel
{
    public Guid Id { get; set; }
    [MaxLength(60)]
    public string Name { get; set; } = default!;
    [MaxLength(11)]
    public string Cpf { get; set; } = default!;
    public DateOnly BirthDate { get; set; }
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
    public EmployeeTypeEnum Type { get; set; }
    public PersonGenderEnum Gender { get; set; }
    public EmployeeStatusEnum Status { get; set; } = EmployeeStatusEnum.Active;
}
