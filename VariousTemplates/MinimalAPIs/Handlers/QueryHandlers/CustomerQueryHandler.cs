namespace MinimalAPIs.Handlers.QueryHandlers;

public sealed record ReadCustomerRequest(int customerId) : IRequest<IResult>;

public sealed class CustomerQueryHandler : IRequestHandler<ReadCustomerRequest, IResult>
{
    private readonly string _connectionString;

    public CustomerQueryHandler(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("MinimalAPIsDB")!;
    }

    public async Task<IResult> Handle(ReadCustomerRequest request, CancellationToken cancellationToken)
    {
        var query = "SELECT * FROM NLog WHERE 1 = @param ";
        try
        {
            using var connection = new SqlConnection(_connectionString);
            var logs = (await connection.QueryAsync<Nlog>(query, new { param = (int?)1 })).ToList();
            return logs.Any() ? Results.Ok(logs) : Results.NotFound();
        }
        catch (Exception ex)
        {
            return Results.Problem(detail: ex.Message, statusCode: 408);
        }
    }
}