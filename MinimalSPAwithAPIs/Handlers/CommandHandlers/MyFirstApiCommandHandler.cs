namespace MinimalSPAwithAPIs.Handlers.CommandHandlers;

public sealed record CreateMyFirstApiCommand(MyFirstApiDTO model) : IRequest<IResult>;
public sealed record UpdateMyFirstApiCommand(MyFirstApiDTO model) : IRequest<IResult>;
public sealed record DeleteMyFirstApiCommand(int id) : IRequest<IResult>;

public sealed class MyFirstApiCommandHandler :
        IRequestHandler<CreateMyFirstApiCommand, IResult>,
        IRequestHandler<UpdateMyFirstApiCommand, IResult>,
        IRequestHandler<DeleteMyFirstApiCommand, IResult>
{
    private readonly ILogger<MyFirstApiCommandHandler> _logger;

    private readonly MyDbContext _db;

    private readonly IMapper _mapper;

    public MyFirstApiCommandHandler(ILogger<MyFirstApiCommandHandler> logger, MyDbContext db, IMapper mapper)
    {
        _logger = logger;
        _db = db;
        _mapper = mapper;
    }

    public async Task<IResult> Handle(CreateMyFirstApiCommand request, CancellationToken cancellationToken)
    {
        await using var dbContextTransaction = await _db.Database.BeginTransactionAsync();

        try
        {
            var nuovoElemento = _mapper.Map<MyFirstApiDbTable>(request.model);
            nuovoElemento.State = "A";
            nuovoElemento.LastUpdateUser = "Temp";
            nuovoElemento.LastUpdateDate = DateTime.Now;
            nuovoElemento.LastUpdateApplication = "readytoworktemplate";

            await _db.MyFirstApiDbTable.AddAsync(nuovoElemento);
            await _db.SaveChangesAsync();
            await dbContextTransaction.CommitAsync();

            return Results.Ok(nuovoElemento);
        }
        catch (Exception)
        {
            await dbContextTransaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<IResult> Handle(UpdateMyFirstApiCommand request, CancellationToken cancellationToken)
    {
        await using var dbContextTransaction = await _db.Database.BeginTransactionAsync();

        try
        {
            var currentEntity = await _db.MyFirstApiDbTable.FindAsync(request.model.PrimaryKey)
                                ?? throw new KeyNotFoundException();

            currentEntity.EndingDate = DateTime.Now;
            currentEntity.State = "C";
            _db.MyFirstApiDbTable.Update(currentEntity);

            var nuovoElemento = _mapper.Map<MyFirstApiDbTable>(request.model);
            nuovoElemento.State = "A";
            nuovoElemento.LastUpdateUser = "Temp";
            nuovoElemento.LastUpdateDate = DateTime.Now;
            nuovoElemento.LastUpdateApplication = "readytoworktemplate";

            await _db.MyFirstApiDbTable.AddAsync(nuovoElemento);
            await _db.SaveChangesAsync();
            await dbContextTransaction.CommitAsync();

            return Results.Ok();
        }
        catch (Exception)
        {
            await dbContextTransaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<IResult> Handle(DeleteMyFirstApiCommand request, CancellationToken cancellationToken)
    {
        await using var dbContextTransaction = await _db.Database.BeginTransactionAsync();

        try
        {
            var entityToDelete = await _db.MyFirstApiDbTable.FindAsync(request.id)
                                ?? throw new KeyNotFoundException();

            entityToDelete.State = "C";
            _db.MyFirstApiDbTable.Update(entityToDelete);

            await _db.SaveChangesAsync();
            await dbContextTransaction.CommitAsync();

            return Results.Ok();
        }
        catch (Exception)
        {
            await dbContextTransaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}