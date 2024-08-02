using BusesControl.Entities.Enums.v1;

namespace BusesControl.Entities.Requests.v1;

public class EmployeeUpdateRequest
{
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
    public PersonGenderEnum Gender { get; set; }
}
