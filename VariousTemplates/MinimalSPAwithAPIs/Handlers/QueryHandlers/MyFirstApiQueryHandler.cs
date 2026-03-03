namespace MinimalSPAwithAPIs.Handlers.QueryHandlers;

public sealed record ReadMyFirstApiQuery(MyFirstApiFilter filter) : IRequest<IResult>;
public sealed record ReadMyFirstApiDapperQuery(MyFirstApiFilter filter) : IRequest<IResult>;

public sealed class MyFirstApiQueryHandler :
    IRequestHandler<ReadMyFirstApiQuery, IResult>,
    IRequestHandler<ReadMyFirstApiDapperQuery, IResult>
{
    private readonly ILogger<MyFirstApiQueryHandler> _logger;

    private readonly MyDbContext _db;

    private readonly IDbConnection _dapper;

    public MyFirstApiQueryHandler(ILogger<MyFirstApiQueryHandler> logger, MyDbContext db, IDbConnection dapper)
    {
        _logger = logger;
        _db = db;
        _dapper = dapper;
    }

    public async Task<IResult> Handle(ReadMyFirstApiQuery request, CancellationToken cancellationToken)
    {
        var query = _db.MyFirstApiDbTable;

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
            new PagedResponse<MyFirstApiDbTable>(results, totalCount, request.filter.PageNumber, request.filter.PageSize);

        return Results.Ok(pagedResponse);
    }

    public async Task<IResult> Handle(ReadMyFirstApiDapperQuery request, CancellationToken cancellationToken)
    {
        var whereClause = QueryableExtensions.BuildWhereClause(request.filter, out var parameters);

        var query = $@"
                            SELECT *
                            FROM MyFirstApiDbTable t
                            {whereClause}
                            ORDER BY {(request.filter.OrderColumnName ?? "t.PrimaryKey") + " " + (request.filter.OrderAscDesc ?? "asc")}
                            OFFSET @Offset ROWS FETCH NEXT @Fetch ROWS ONLY
                            ";

        parameters.Add("@Offset", (request.filter.PageNumber - 1) * request.filter.PageSize);
        parameters.Add("@Fetch", request.filter.PageSize);

        var results = await _dapper.QueryAsync<dynamic>(query, parameters);

        var pagedResponse = new PagedResponse<dynamic>
        {
            PageNumber = request.filter.PageNumber,
            PageSize = request.filter.PageSize,
            TotalCount = results.Count(),
            Results = results
        };

        return Results.Ok(pagedResponse);
    }
}