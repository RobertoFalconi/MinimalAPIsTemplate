namespace MinimalAPIs.Handlers.CommandHandlers;

public record CreateCustomerRequest(CustomerAPI customer) : IRequest<bool>;

public class CustomerCommandHandler :
    IRequestHandler<CreateCustomerRequest, bool>
{
    private readonly string connectionString;

    public CustomerCommandHandler(IConfiguration configuration)
    {
        connectionString = configuration.GetConnectionString("MinimalAPIsDB")!;
    }

    public async Task<bool> Handle(CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        var query = "SELECT * FROM NLog WHERE 1 = @param ";
        using var connection = new SqlConnection(connectionString);
        var logs = (await connection.QueryAsync<Nlog>(query, new { param = (int?)1 })).ToList();
        return true;
    }
}