using BusesControl.Entities.Enums.v1;

namespace BusesControl.Entities.Responses.v1;

public class CustomerResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Cpf { get; set; }
    public string? Cnpj { get; set; }
    public DateOnly? BirthDate { get; set; }
    public DateOnly? OpenDate { get; set; }
    public string Email { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string HomeNumber { get; set; } = default!;
    public string Logradouro { get; set; } = default!;
    public string ComplementResidential { get; set; } = default!;
    public string Neighborhood { get; set; } = default!;
    public string City { get; set; } = default!;
    public string State { get; set; } = default!;
    public bool Active { get; set; }
    public CustomerTypeEnum Type { get; set; }
    public bool InCompliance { get; set; }
}
