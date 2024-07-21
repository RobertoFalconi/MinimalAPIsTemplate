namespace MinimalSPAwithAPIs.Models.Filters;

public class PagedResponse<T>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get { return (int)Math.Ceiling(((double)TotalCount / (double)PageSize)); } }
    public int TotalCount { get; set; }
    public IEnumerable<T> Results { get; set; }

    public PagedResponse()
    {
        this.Results = new HashSet<T>();
    }

    public PagedResponse(List<T> results, int totalCount, int pageNumber, int pageSize)
    {
        this.Results = results;
        this.TotalCount = totalCount;
        this.PageNumber = pageNumber;
        this.PageSize = pageSize;
    }
}