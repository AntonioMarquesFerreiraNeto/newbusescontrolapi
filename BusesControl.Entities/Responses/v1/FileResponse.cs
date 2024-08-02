namespace BusesControl.Entities.Responses.v1;

public class FileResponse
{
    public byte[] FileContent { get; set; } = default!;
    public string FileName { get; set; } = default!;
    public string ContentType { get; set; } = default!;
}
