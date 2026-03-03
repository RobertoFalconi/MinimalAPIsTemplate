namespace MinimalSPAwithAPIs.Models.Filters;

public class Filter
{
    public Filter()
    {
        this.PageNumber = 1;
        this.PageSize = 10;
    }

    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string? OrderAscDesc { get; set; }
    public string? OrderColumnName { get; set; }
}
