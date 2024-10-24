using NotificationService.Entities;
using NotificationService.Entities.ValueObjects;

namespace NotificationService.BackgroundServices;

public interface INotificationSender
{
    Task SendAsync(MessageData message, NotificationSettings notificationSetting, CancellationToken cancellationToken);
    bool CanSend(NotificationSettings notificationSetting,  CancellationToken cancellationToken);
}