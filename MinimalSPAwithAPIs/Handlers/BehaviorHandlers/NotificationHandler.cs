namespace MinimalSPAwithAPIs.Handlers.BehaviorHandlers;

public class NotificationHandler<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly MyDbContext _dbContext;

    public NotificationHandler(MyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // Eseguire il comando
        var response = await next();

        // Creare e salvare la notifica
        var notification = new
        {
            CommandType = request.GetType().Name,
            Timestamp = DateTime.Now,
            // Aggiungi ulteriori dettagli della notifica come necessario
        };

        // _dbContext.Notifications.Add(notification);
        //await _dbContext.SaveChangesAsync(cancellationToken);

        return response;
    }
}
