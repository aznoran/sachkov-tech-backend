using CSharpFunctionalExtensions;
using NotificationService.Entities;
using NotificationService.Entities.ValueObjects;
using NotificationService.HelperClasses;

namespace NotificationService.Services;

public interface INotificationSender
{
    Task<UnitResult<Error>> SendAsync(MessageData message, NotificationSettings notificationSetting,
        CancellationToken cancellationToken);

    bool CanSend(NotificationSettings notificationSetting, CancellationToken cancellationToken);
}