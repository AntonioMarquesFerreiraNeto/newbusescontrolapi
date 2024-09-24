using System.Text.Json.Serialization;

namespace BusesControl.Entities.DTOs;

public class LegalEntityDetailsDTO
{
    [JsonPropertyName("razao_social")]
    public string RazaoSocial { get; set; } = default!;

    [JsonPropertyName("nome_fantasia")]
    public string? Name { get; set; }

    [JsonPropertyName("porte")]
    public string TypeSize { get; set; } = default!;

    [JsonPropertyName("capital_social")]
    public decimal CapitalSocial { get; set; }

    [JsonPropertyName("natureza_juridica")]
    public string LegalNature { get; set; } = default!;

    [JsonPropertyName("uf")]
    public string Uf { get; set; } = default!;
    
    [JsonPropertyName("cep")]
    public string ZipCode { get; set; } = default!;
    
    [JsonPropertyName("bairro")]
    public string Neighborhood { get; set; } = default!;
    
    [JsonPropertyName("numero")]
    public string Number { get; set; } = default!;
    
    [JsonPropertyName("municipio")]
    public string City { get; set; } = default!;
    
    [JsonPropertyName("logradouro")]
    public string Logradouro { get; set; } = default!;
    
    [JsonPropertyName("complemento")]
    public string? Complement { get; set; }
    
    [JsonPropertyName("descricao_situacao_cadastral")]
    public string Status { get; set; } = default!;
}
