using BusesControl.Entities.Enums.v1;

namespace BusesControl.Entities.Responses.v1;

public class EmployeeResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Cpf { get; set; } = default!;
    public DateOnly BirthDate { get; set; }
    public string Email { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string HomeNumber { get; set; } = default!;
    public string Logradouro { get; set; } = default!;
    public string ComplementResidential { get; set; } = default!;
    public string Neighborhood { get; set; } = default!;
    public string City { get; set; } = default!;
    public string State { get; set; } = default!;
    public EmployeeTypeEnum Type { get; set; }
    public EmployeeStatusEnum Status { get; set; }
    public PersonGenderEnum Gender { get; set; }
}
