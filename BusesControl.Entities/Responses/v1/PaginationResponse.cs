namespace BusesControl.Entities.Responses.v1;

public class PaginationResponse<T>
{
    public int TotalSize { get; set; }
    public IEnumerable<T> Response { get; set; } = default!;
}
