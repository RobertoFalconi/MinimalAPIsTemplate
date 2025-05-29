namespace MVCwithMediatRandCQRS.QueryHandlers;

public sealed record GetUtentiQuery() : IRequest<List<UtenteViewModel>>;
public sealed record GetUtenteByIdQuery(int Id) : IRequest<UtenteViewModel>;

public sealed class UtenteQueryHandler :
    IRequestHandler<GetUtentiQuery, List<UtenteViewModel>>,
    IRequestHandler<GetUtenteByIdQuery, UtenteViewModel>
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

    public async Task<List<UtenteViewModel>> Handle(GetUtentiQuery request, CancellationToken cancellationToken)
    {
        var query = $@" SELECT *
                        FROM Utenti ";

        var utenti = new List<UtenteViewModel>();

        //using (var conn = new SqlConnection(_connectionString))
        //{
        //    utenti = (await conn.QueryAsync<UtenteEntity>(query, new { request })).ToList();
        //}

        return utenti;
    }

    public async Task<UtenteViewModel> Handle(GetUtenteByIdQuery request, CancellationToken cancellationToken)
    {
        var query = $@" SELECT *
                        FROM Utenti 
                        WHERE Id = {request.Id} ";

        var utente = new UtenteViewModel();

        // MOCK
        utente.Nome = "Mario";
        utente.Cognome = "Rossi";
        utente.Email = "mariorossi@prova.it";
        //using (var conn = new SqlConnection(_connectionString))
        //{
        //    utente = await conn.QuerySingleOrDefaultAsync<UtenteEntity>(query, new { request });
        //}

        return utente;
    }
}