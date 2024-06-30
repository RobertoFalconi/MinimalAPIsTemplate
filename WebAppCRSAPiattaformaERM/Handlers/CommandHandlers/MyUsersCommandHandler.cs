namespace MinimalSPAwithAPIs.Handlers.CommandHandlers;

public sealed record InserisciMyUsersCommand(MyUsersDTO model) : IRequest<IResult>;
public sealed record AggiornaMyUsersCommand(MyUsersDTO model) : IRequest<IResult>;
public sealed record RimuoviMyUsersCommand(int id) : IRequest<IResult>;

public sealed class MyUsersCommandHandler :
        IRequestHandler<InserisciMyUsersCommand, IResult>,
        IRequestHandler<AggiornaMyUsersCommand, IResult>,
        IRequestHandler<RimuoviMyUsersCommand, IResult>
{
    private readonly ILogger<MyUsersCommandHandler> _logger;

    private readonly MyDbContext _db; 
    
    private readonly IMapper _mapper;

    public MyUsersCommandHandler(ILogger<MyUsersCommandHandler> logger, MyDbContext db, IMapper mapper)
    {
        _logger = logger;
        _db = db;
        _mapper = mapper;
    }

    public async Task<IResult> Handle(InserisciMyUsersCommand request, CancellationToken cancellationToken)
    {
        var validator = new MyUsersValidator();
        var results = validator.Validate(request.model);

        if (!results.IsValid)
        {
            var errori = "Errori: ";
            foreach (var failure in results.Errors)
            {
                errori += $"{failure.ErrorMessage} ";
            }
            return Results.BadRequest(errori);
        }
        using (var dbContextTransaction = await _db.Database.BeginTransactionAsync())
        {
            try
            {
                var nuovoElemento = _mapper.Map<MyUsersDb>(request.model);
                nuovoElemento.Role = "Role4";
                nuovoElemento.State = "A";
                nuovoElemento.LastUpdateUser = "Temp";
                nuovoElemento.LastUpdateDate = DateTime.Now;
                nuovoElemento.LastUpdateApp = "readytoworktemplate";

                await _db.MyUsers.AddAsync(nuovoElemento);
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

    public async Task<IResult> Handle(AggiornaMyUsersCommand request, CancellationToken cancellationToken)
    {
        using (var dbContextTransaction = await _db.Database.BeginTransactionAsync())
        {
            try
            {
                var currentEntity = await _db.MyUsers.FindAsync(request.model.PrimaryKey);

                if (currentEntity != null)
                {
                    currentEntity.EndingDate = DateTime.Now;
                    currentEntity.State = "C";
                    _db.MyUsers.Update(currentEntity);

                    var nuovoElemento = _mapper.Map<MyUsersDb>(request.model);
                    nuovoElemento.Role = "Role4";
                    nuovoElemento.State = "A";
                    nuovoElemento.LastUpdateUser = "Temp";
                    nuovoElemento.LastUpdateDate = DateTime.Now;
                    nuovoElemento.LastUpdateApp = "readytoworktemplate";

                    await _db.MyUsers.AddAsync(nuovoElemento);
                    await _db.SaveChangesAsync();
                    await dbContextTransaction.CommitAsync();
                    return Results.Ok(currentEntity);
                }

                return Results.Empty;
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

    public async Task<IResult> Handle(RimuoviMyUsersCommand request, CancellationToken cancellationToken)
    {
        using (var dbContextTransaction = await _db.Database.BeginTransactionAsync())
        {
            try
            {
                var entityToDelete = await _db.MyUsers.FindAsync(request.id);
                if (entityToDelete != null)
                {
                    entityToDelete.State = "C";
                    _db.MyUsers.Update(entityToDelete);

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