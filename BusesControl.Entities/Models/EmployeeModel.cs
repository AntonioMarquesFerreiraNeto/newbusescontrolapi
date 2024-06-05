using BusesControl.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace BusesControl.Entities.Models;

public class EmployeeModel
{
    public Guid Id { get; set; }
    [MinLength(60)]
    public string? Name { get; set; }
    [MaxLength(11)]
    public string? Cpf { get; set; }
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
    [MaxLength(20)]
    public string Surname { get; set; } = default!;
    public string? KeyRedefinition { get; set; } = null;
    public EmployeeTypeEnum Type { get; set; }
    public EmployeeStatusEnum Status { get; set; } = EmployeeStatusEnum.Active;
    public UserStatusEnum UserStatus { get; set; } = UserStatusEnum.Inactive;
}
