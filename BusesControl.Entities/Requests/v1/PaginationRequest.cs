﻿namespace BusesControl.Entities.Requests.v1;

public class PaginationRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Search { get; set; } = null;
}
