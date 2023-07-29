namespace MinimalAPIs.Handlers.QueryHandlers;

public record ReadCustomerRequest(CustomerAPI customer) : IRequest<bool>;

public class CustomerQueryHandler :
    IRequestHandler<ReadCustomerRequest, bool>
{
    private readonly string connectionString;

    public CustomerQueryHandler(IConfiguration configuration)
    {
        connectionString = configuration.GetConnectionString("MinimalAPIsDB")!;
    }

    public async Task<bool> Handle(ReadCustomerRequest request, CancellationToken cancellationToken)
    {
        var query = "SELECT * FROM NLog WHERE 1 = @param ";
        using var connection = new SqlConnection(connectionString);
        var logs = (await connection.QueryAsync<Nlog>(query, new { param = (int?)1 })).ToList();
        return true;
    }
}