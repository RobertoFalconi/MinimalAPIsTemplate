﻿namespace MVCwithMediatRandCQRS.Handlers;

public class NotificationHandler<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    //private readonly MyDbContext _dbContext;

    public NotificationHandler(/*MyDbContext dbContext*/)
    {
        //_dbContext = dbContext;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = await next();

        var notification = new
        {
            CommandType = request.GetType().Name,
            Timestamp = DateTime.Now
        };

        // _dbContext.Notifications.Add(notification);
        //await _dbContext.SaveChangesAsync(cancellationToken);

        return response;
    }
}