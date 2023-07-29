namespace MinimalAPIs.Handlers.QueryHandlers;

public record MyRequest(string Message) : IRequest<List<Nlog>>;

public class MyRequestHandler :
    IRequestHandler<MyRequest, List<Nlog>>
{
    private readonly string connectionString;

    public MyRequestHandler(IConfiguration configuration)
    {
        connectionString = configuration.GetConnectionString("MinimalAPIsDB");
    }

    public async Task<List<Nlog>> Handle(MyRequest request, CancellationToken cancellationToken)
    {
        var query = "SELECT * FROM NLog WHERE 1 = @param ";
        using var connection = new SqlConnection(connectionString);
        var logs = (await connection.QueryAsync<Nlog>(query, new { param = (int?)1 })).ToList();
        return logs;
    }
}