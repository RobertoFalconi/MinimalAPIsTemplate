namespace MinimalSPAwithAPIs.Handlers.QueryHandlers;

public sealed record ReadMyFirstApiQuery(MyFirstApiFilter filter) : IRequest<IResult>;

public sealed class MyFirstApiQueryHandler :
    IRequestHandler<ReadMyFirstApiQuery, IResult>
{
    private readonly ILogger<MyFirstApiQueryHandler> _logger;

    private readonly MyDbContext _db;

    public MyFirstApiQueryHandler(ILogger<MyFirstApiQueryHandler> logger, MyDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    public async Task<IResult> Handle(ReadMyFirstApiQuery request, CancellationToken cancellationToken)
    {
        var query = _db.MyFirstApiDb;

        var filteredQuery = QueryableExtensions
            .ApplyFilter(query, request.filter)
            .Where(x => x.State != "C");

        var totalCount = await filteredQuery
            .CountAsync();

        var results = await filteredQuery
            .OrderBy((request.filter.OrderColumnName ?? $"{nameof(request.filter.PrimaryKey)}") + " " + (request.filter.OrderAscDesc ?? "asc"))
            .Skip((request.filter.PageNumber - 1) * request.filter.PageSize)
            .Take(request.filter.PageSize)
            .ToListAsync();

        var pagedResponse =
            new PagedResponse<MyFirstApiDb>(results, totalCount, request.filter.PageNumber, request.filter.PageSize);

        return Results.Ok(pagedResponse);
    }
}