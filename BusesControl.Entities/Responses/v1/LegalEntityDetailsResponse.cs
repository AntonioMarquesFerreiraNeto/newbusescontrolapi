namespace BusesControl.Entities.Responses.v1;

public class LegalEntityDetailsResponse
{
    public string RazaoSocial { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string TypeSize { get; set; } = default!;
    public decimal CapitalSocial { get; set; }
    public string LegalNature { get; set; } = default!;
    public string Uf { get; set; } = default!;
    public string ZipCode { get; set; } = default!;
    public string Neighborhood { get; set; } = default!;
    public string Number { get; set; } = default!;
    public string City { get; set; } = default!;
    public string Logradouro { get; set; } = default!;
    public string? Complement { get; set; }
    public string Status { get; set; } = default!;
}
