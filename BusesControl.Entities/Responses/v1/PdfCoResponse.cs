using System.Text.Json.Serialization;

namespace BusesControl.Entities.Responses.v1;

public class PdfCoResponse
{
    [JsonPropertyName("url")]
    public string Url { get; set; } = default!;
}
