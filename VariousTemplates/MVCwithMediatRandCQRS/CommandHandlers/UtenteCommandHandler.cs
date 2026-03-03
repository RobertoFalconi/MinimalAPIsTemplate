namespace MVCwithMediatRandCQRS.CommandHandlers;

public sealed record CreateUtenteCommand(string Nome, string Cognome, string Email) : IRequest<int>;
public sealed record UpdateUtenteCommand(int Id, string Email) : IRequest<bool>;
public sealed record DeleteUtenteCommand(int Id) : IRequest<bool>;

public sealed class UtenteCommandHandler :
        IRequestHandler<CreateUtenteCommand, int>,
        IRequestHandler<UpdateUtenteCommand, bool>,
        IRequestHandler<DeleteUtenteCommand, bool>
{
    private readonly ILogger<UtenteCommandHandler> _logger;

    private readonly MyDbContext _db;

    public UtenteCommandHandler(ILogger<UtenteCommandHandler> logger, MyDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    public async Task<int> Handle(CreateUtenteCommand request, CancellationToken cancellationToken)
    {
        //await using var dbContextTransaction = await _db.Database.BeginTransactionAsync();

        var nuovoElemento = new Utenti();
        nuovoElemento.Nome = request.Nome;
        nuovoElemento.Cognome = request.Cognome;
        nuovoElemento.Email = request.Email;

        //await _db.Utenti.AddAsync(nuovoElemento);
        //await _db.SaveChangesAsync();
        //await dbContextTransaction.CommitAsync();

        // mocked return
        nuovoElemento.Id = 7;
        return nuovoElemento.Id;
    }

    public async Task<bool> Handle(UpdateUtenteCommand request, CancellationToken cancellationToken)
    {
        await using var dbContextTransaction = await _db.Database.BeginTransactionAsync();

        var currentEntity = await (from u in _db.Utenti
                                   where u.Id == request.Id
                                   select u).SingleAsync()
                                   ?? throw new KeyNotFoundException();

        currentEntity.Email = request.Email;

        _db.Utenti.Update(currentEntity);
        await _db.SaveChangesAsync();
        await dbContextTransaction.CommitAsync();

        return true;
    }

    public async Task<bool> Handle(DeleteUtenteCommand request, CancellationToken cancellationToken)
    {
        await using var dbContextTransaction = await _db.Database.BeginTransactionAsync();

        var currentEntity = await (from u in _db.Utenti
                                   where u.Id == request.Id
                                   select u).SingleAsync()
                                   ?? throw new KeyNotFoundException();

        _db.Utenti.Remove(currentEntity);
        await _db.SaveChangesAsync();
        await dbContextTransaction.CommitAsync();

        return true;
    }
}