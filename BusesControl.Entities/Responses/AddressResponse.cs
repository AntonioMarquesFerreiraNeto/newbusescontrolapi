namespace BusesControl.Entities.Response;

public class AddressResponse
{
    public string Cep { get; set; } = default!;
    public string Logradouro { get; set; } = default!;
    public string Complemento { get; set; } = default!;
    public string Bairro { get; set; } = default!;
    public string Localidade { get; set; } = default!;
    public string Uf { get; set; } = default!;
    public string Ibge { get; set; } = default!;
    public string Gia { get; set; } = default!;
    public string Ddd { get; set; } = default!;
    public string Siafi { get; set; } = default!;
}
