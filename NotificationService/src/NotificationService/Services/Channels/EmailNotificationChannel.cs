using CSharpFunctionalExtensions;
using NotificationService.Entities;
using NotificationService.Entities.ValueObjects;
using NotificationService.HelperClasses;

namespace NotificationService.BackgroundServices.Channels;

public class EmailNotificationChannel : INotificationSender
{
    public Task<UnitResult<Error>> SendAsync(MessageData message, NotificationSettings notificationSetting, CancellationToken cancellationToken)
    {
        return Task.FromResult(UnitResult.Success<Error>());
    }

    public bool CanSend(NotificationSettings notificationSetting, CancellationToken cancellationToken)
    {
        return true;
    }
}