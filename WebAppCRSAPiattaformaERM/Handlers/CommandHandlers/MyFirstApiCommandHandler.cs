namespace MinimalSPAwithAPIs.Handlers.CommandHandlers;

public sealed record InserisciMyFirstApiCommand(MyFirstApiDTO model) : IRequest<IResult>;
public sealed record AggiornaMyFirstApiCommand(MyFirstApiDTO model) : IRequest<IResult>;
public sealed record RimuoviMyFirstApiCommand(int id) : IRequest<IResult>;

public sealed class MyFirstApiCommandHandler :
        IRequestHandler<InserisciMyFirstApiCommand, IResult>,
        IRequestHandler<AggiornaMyFirstApiCommand, IResult>,
        IRequestHandler<RimuoviMyFirstApiCommand, IResult>
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

    public async Task<IResult> Handle(InserisciMyFirstApiCommand request, CancellationToken cancellationToken)
    {

        using (var dbContextTransaction = await _db.Database.BeginTransactionAsync())
        {
            try
            {
                var nuovoElemento = _mapper.Map<MyFirstApiDb>(request.model);
                nuovoElemento.State = "A";
                nuovoElemento.LastUpdateUser = "Temp";
                nuovoElemento.LastUpdateDate = DateTime.Now;
                nuovoElemento.LastUpdateApplication = "readytoworktemplate";

                await _db.MyFirstApiDb.AddAsync(nuovoElemento);
                await _db.SaveChangesAsync();
                await dbContextTransaction.CommitAsync();

                return Results.Ok(nuovoElemento);
            }
            catch (Exception ex)
            {
                var errorMessage = ex.InnerException != null ? $"{ex.Message} Inner exception: {ex.InnerException.Message}" : ex.Message;
                _logger.LogError(errorMessage);
                await dbContextTransaction.RollbackAsync(cancellationToken);
                return Results.BadRequest(errorMessage);
            }
        }
    }

    public async Task<IResult> Handle(AggiornaMyFirstApiCommand request, CancellationToken cancellationToken)
    {
        using (var dbContextTransaction = await _db.Database.BeginTransactionAsync())
        {
            try
            {
                var currentEntity = await _db.MyFirstApiDb.FindAsync(request.model.PrimaryKey);
                
                if (currentEntity != null)
                {
                    currentEntity.EndingDate = DateTime.Now;
                    currentEntity.State = "C";
                    _db.MyFirstApiDb.Update(currentEntity);

                    var nuovoElemento = _mapper.Map<MyFirstApiDb>(request.model);
                    nuovoElemento.State = "A";
                    nuovoElemento.LastUpdateUser = "Temp";
                    nuovoElemento.LastUpdateDate = DateTime.Now;
                    nuovoElemento.LastUpdateApplication = "readytoworktemplate";

                    await _db.MyFirstApiDb.AddAsync(nuovoElemento);
                    await _db.SaveChangesAsync();
                    await dbContextTransaction.CommitAsync();
                }

                return Results.Ok();
            }
            catch (Exception ex)
            {
                var errorMessage = ex.InnerException != null ? $"{ex.Message} Inner exception: {ex.InnerException.Message}" : ex.Message;
                _logger.LogError(errorMessage);
                await dbContextTransaction.RollbackAsync(cancellationToken);
                return Results.BadRequest(errorMessage);
            }
        }
    }

    public async Task<IResult> Handle(RimuoviMyFirstApiCommand request, CancellationToken cancellationToken)
    {
        using (var dbContextTransaction = await _db.Database.BeginTransactionAsync())
        {
            try
            {
                var entityToDelete = await _db.MyFirstApiDb.FindAsync(request.id);
                if (entityToDelete != null)
                {
                    entityToDelete.State = "C";
                    _db.MyFirstApiDb.Update(entityToDelete);

                    await _db.SaveChangesAsync();
                    await dbContextTransaction.CommitAsync();
                }

                return Results.Ok();
            }
            catch (Exception ex)
            {
                var errorMessage = ex.InnerException != null ? $"{ex.Message} Inner exception: {ex.InnerException.Message}" : ex.Message;
                _logger.LogError(errorMessage);
                await dbContextTransaction.RollbackAsync(cancellationToken);
                return Results.BadRequest(errorMessage);
            }
        }
    }
}