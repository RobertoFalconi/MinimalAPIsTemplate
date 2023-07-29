namespace MinimalAPIs.Handlers.QueryHandlers;

public record ReadCustomerRequest(int customerId) : IRequest<IResult>;

public class CustomerQueryHandler :
    IRequestHandler<ReadCustomerRequest, IResult>
{
    private readonly string connectionString;

    public CustomerQueryHandler(IConfiguration configuration)
    {
        connectionString = configuration.GetConnectionString("MinimalAPIsDB")!;
    }

    public async Task<IResult> Handle(ReadCustomerRequest request, CancellationToken cancellationToken)
    {
        var query = "SELECT * FROM NLog WHERE 1 = @param ";
        try
        {
            using var connection = new SqlConnection(connectionString);
            var logs = (await connection.QueryAsync<Nlog>(query, new { param = (int?)1 })).ToList();
            return logs.Any() ? Results.Ok(logs) : Results.NotFound();
        }
        catch (Exception ex)
        {
            return Results.Problem(detail: ex.Message, statusCode: 408);
        }
    }
}