namespace BusesControl.Entities.Responses;

public class FileResponse
{
    public byte[] FileContent { get; set; } = default!;
    public string FileName { get; set; } = default!;
    public string ContentType { get; set; } = default!;
}
