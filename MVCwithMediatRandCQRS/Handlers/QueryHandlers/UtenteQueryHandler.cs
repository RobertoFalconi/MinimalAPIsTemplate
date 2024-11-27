namespace MVCwithMediatRandCQRS.Web.Handlers.QueryHandlers;

public sealed record GetUtentiQuery(UtenteServiceRequest ServiceRequest) : IRequest<List<UtenteEntity>>;
public sealed record GetUtenteByIdQuery(UtenteServiceRequest ServiceRequest) : IRequest<UtenteEntity>;

public sealed class UtenteQueryHandler :
    IRequestHandler<GetUtentiQuery, List<UtenteEntity>>,
    IRequestHandler<GetUtenteByIdQuery, UtenteEntity>
{
    private readonly IConfiguration _configuration;

    private readonly ILogger<UtenteQueryHandler> _logger;

    private readonly string _connectionString;

    public UtenteQueryHandler(IConfiguration configuration, ILogger<UtenteQueryHandler> logger)
    {
        _configuration = configuration;
        _logger = logger;
        _connectionString = configuration.GetConnectionString("MVCwithMediatRandCQRS.DB.Main")!;
    }

    public async Task<List<UtenteEntity>> Handle(GetUtentiQuery request, CancellationToken cancellationToken)
    {
        var query = $@" SELECT *
                        FROM Utenti ";

        var utenti = new List<UtenteEntity>();

        //using (var conn = new SqlConnection(_connectionString))
        //{
        //    utenti = (await conn.QueryAsync<UtenteEntity>(query, new { request })).ToList();
        //}

        return utenti;
    }

    public async Task<UtenteEntity> Handle(GetUtenteByIdQuery request, CancellationToken cancellationToken)
    {
        var query = $@" SELECT *
                        FROM Utenti 
                        WHERE Id = {request.ServiceRequest.Id} ";

        var utente = new UtenteEntity();

        //using (var conn = new SqlConnection(_connectionString))
        //{
        //    utente = await conn.QuerySingleOrDefaultAsync<UtenteEntity>(query, new { request });
        //}

        return utente;
    }
}