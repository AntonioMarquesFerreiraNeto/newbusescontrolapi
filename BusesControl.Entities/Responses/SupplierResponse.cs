namespace BusesControl.Entities.Responses;

public class SupplierResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Cnpj { get; set; } = default!;
    public DateOnly OpenDate { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string HomeNumber { get; set; } = default!;
    public string Logradouro { get; set; } = default!;
    public string ComplementResidential { get; set; } = default!;
    public string Neighborhood { get; set; } = default!;
    public string City { get; set; } = default!;
    public string State { get; set; } = default!;
    public bool Active { get; set; }
}
