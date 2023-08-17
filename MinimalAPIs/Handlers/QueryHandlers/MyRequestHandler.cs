namespace MinimalAPIs.Handlers.QueryHandlers;

public sealed record MyRequest(string Message) : IRequest<List<Nlog>>;

public sealed class MyRequestHandler : IRequestHandler<MyRequest, List<Nlog>>
{
    private readonly string _connectionString;

    public MyRequestHandler(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("MinimalAPIsDB")!;
    }

    public async Task<List<Nlog>> Handle(MyRequest request, CancellationToken cancellationToken)
    {
        var query = "SELECT * FROM NLog WHERE 1 = @param ";
        using var connection = new SqlConnection(_connectionString);
        var logs = (await connection.QueryAsync<Nlog>(query, new { param = (int?)1 })).ToList();
        return logs;
    }
}