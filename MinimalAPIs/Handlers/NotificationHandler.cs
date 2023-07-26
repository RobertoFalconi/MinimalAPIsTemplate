namespace MinimalAPIs.Handlers; 

public record Notification(string Message) : INotification;

public class NotificationHandler : INotificationHandler<Notification>
{
    public async Task Handle(Notification notification,CancellationToken cancellationToken)
    {
        // Some logic to handle the notification
    }
}