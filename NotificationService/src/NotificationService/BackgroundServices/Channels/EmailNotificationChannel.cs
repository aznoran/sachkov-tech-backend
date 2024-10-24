using NotificationService.Entities;

namespace NotificationService.BackgroundServices.Channels;

public class EmailNotificationChannel : INotificationSender
{
    public Task SendAsync(MessageData message, NotificationSettings notificationSetting, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public bool CanSend(NotificationSettings notificationSetting, CancellationToken cancellationToken)
    {
        return true;
    }
}