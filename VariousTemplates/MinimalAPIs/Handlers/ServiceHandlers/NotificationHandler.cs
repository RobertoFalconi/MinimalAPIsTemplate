namespace MinimalAPIs.Handlers.ServiceHandlers;

public sealed record Notification(string Message) : INotification;

public sealed class NotificationHandler : INotificationHandler<Notification>
{
    public async Task Handle(Notification notification, CancellationToken cancellationToken)
    {
        // Some logic to handle the notification
    }
}