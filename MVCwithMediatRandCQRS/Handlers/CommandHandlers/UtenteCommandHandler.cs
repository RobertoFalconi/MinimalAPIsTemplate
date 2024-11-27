namespace MVCwithMediatRandCQRS.Web.Handlers.CommandHandlers;

public sealed record CreateUtenteCommand(UtenteServiceRequest ServiceRequest) : IRequest<int>;
public sealed record UpdateUtenteCommand(UtenteServiceRequest ServiceRequest) : IRequest<bool>;
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
        await using var dbContextTransaction = await _db.Database.BeginTransactionAsync();

        var nuovoElemento = new Utenti();
        nuovoElemento.Nome = "Mario";
        nuovoElemento.Cognome = "Rossi";
        nuovoElemento.CodiceFiscale = "ABC";
        nuovoElemento.Email = "mariorossi@prova.it";
        nuovoElemento.Telefono = "123456789";
        nuovoElemento.Ruolo = "Admin";

        await _db.Utenti.AddAsync(nuovoElemento);
        await _db.SaveChangesAsync();
        await dbContextTransaction.CommitAsync();

        return nuovoElemento.Id;
    }

    public async Task<bool> Handle(UpdateUtenteCommand request, CancellationToken cancellationToken)
    {
        await using var dbContextTransaction = await _db.Database.BeginTransactionAsync();

        var currentEntity = await (from u in _db.Utenti
                                   where u.Id == request.ServiceRequest.Id
                                   select u).SingleAsync()
                                   ?? throw new KeyNotFoundException();

        currentEntity.Telefono = request.ServiceRequest.Telefono;

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