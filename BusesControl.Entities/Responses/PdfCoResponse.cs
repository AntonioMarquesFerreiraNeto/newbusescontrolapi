using System.Text.Json.Serialization;

namespace BusesControl.Entities.Responses;

public class PdfCoResponse
{
    [JsonPropertyName("url")]
    public string Url { get; set; } = default!;
}
