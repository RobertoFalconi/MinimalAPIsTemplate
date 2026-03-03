namespace MinimalAPIs.Handlers.CommandHandlers;

public sealed record CreateCustomerRequest(CustomerAPI customer) : IRequest<IResult>;

public sealed class CustomerCommandHandler : IRequestHandler<CreateCustomerRequest, IResult>
{
    private readonly IDbContextFactory<MinimalApisDbContext> _dbContextFactory;

    public CustomerCommandHandler(IConfiguration configuration, IDbContextFactory<MinimalApisDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<IResult> Handle(CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        try
        {
            using (var context = await _dbContextFactory.CreateDbContextAsync())
            {
                using var dbContextTransaction = await context.Database.BeginTransactionAsync();
                var newLog = new Nlog();
                var res = await context.Nlog.AddAsync(newLog);
            }
            return Results.Ok("Your customer has been created");
        }
        catch (Exception ex)
        {
            return Results.Problem(detail: ex.Message, statusCode: 409);
        }
    }
}