namespace MinimalSPAwithAPIs.Handlers.QueryHandlers;

public sealed record GetMyUsersQuery(MyUsersFilter filter) : IRequest<IResult>;

public sealed record GetCurrentUser(ClaimsPrincipal User) : IRequest<IResult>;

public sealed class MyUsersQueryHandler :
    IRequestHandler<GetMyUsersQuery, IResult>,
    IRequestHandler<GetCurrentUser, IResult>
{
    private readonly ILogger<MyUsersQueryHandler> _logger;

    private readonly MyDbContext _db;

    public MyUsersQueryHandler(ILogger<MyUsersQueryHandler> logger, HttpClient httpClient, MyDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    public async Task<IResult> Handle(GetMyUsersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _db.MyUsers;

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
                new PagedResponse<MyUsersDb>(results, totalCount, request.filter.PageNumber, request.filter.PageSize);

            return Results.Ok(pagedResponse);
        }
        catch (Exception ex)
        {
            var errorMessage = ex.InnerException != null ? $"{ex.Message} Inner exception: {ex.InnerException.Message}" : ex.Message;
            _logger.LogError(errorMessage);
            return Results.BadRequest(errorMessage);
        }
    }

    public async Task<IResult> Handle(GetCurrentUser request, CancellationToken cancellationToken)
    {
        try
        {
            var utente = new MyUser();
            utente.NumeroMatricola = request.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            utente.CodiceFiscale = request.User.FindFirst(IdmClaimTypes.CodiceFiscale)?.Value;
            utente.IndirizzoEmail = request.User.FindFirst(ClaimTypes.Email)?.Value;
            utente.WindowsAccount = request.User.FindFirst(ClaimTypes.WindowsAccountName)?.Value;
            utente.Nominativo = request.User.FindFirst(IdmClaimTypes.Nominativo)?.Value;
            utente.Nome = request.User.FindFirst(ClaimTypes.Name)?.Value;
            utente.Cognome = request.User.FindFirst(ClaimTypes.Surname)?.Value;
            utente.CodiceSede = request.User.FindFirst(IdmClaimTypes.CodiceSede)?.Value;
            utente.CodiceSedeSap = request.User.FindFirst(IdmClaimTypes.CodiceSedeSap)?.Value;
            utente.ProfiloVega = request.User.FindFirst(IdmClaimTypes.ProfiloVega)?.Value;
            utente.Qualifica = request.User.FindFirst(IdmClaimTypes.Qualifica)?.Value;
            utente.Ruoli = request.User.FindAll(ClaimTypes.Role).Select(x => x.Value).ToList();

            return Results.Ok(utente);
        }
        catch (Exception ex)
        {
            var errorMessage = ex.InnerException != null ? $"{ex.Message} Inner exception: {ex.InnerException.Message}" : ex.Message;
            _logger.LogError(errorMessage);
            return Results.BadRequest(errorMessage);
        }
    }
}

public class MyUser
{
    public string? NumeroMatricola { get; set; }
    public string? CodiceFiscale { get; set; }
    public string? IndirizzoEmail { get; set; }
    public string? WindowsAccount { get; set; }
    public string? Nominativo { get; set; }
    public string? Nome { get; set; }
    public string? Cognome { get; set; }
    public string? CodiceSede { get; set; }
    public string? CodiceSedeSap { get; set; }
    public string? ProfiloVega { get; set; }
    public string? Qualifica { get; set; }
    public List<string>? Ruoli { get; set; }
}