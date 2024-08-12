namespace BusesControl.Entities.Requests.v1;

public class PaginationRequest
{
    private int _pageSize = 10;
    public int Page { get; set; } = 1;
    public string? Search { get; set; } = null;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > 100) ? 100 : value;
    }
}
