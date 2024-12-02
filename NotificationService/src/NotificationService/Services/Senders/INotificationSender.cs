using CSharpFunctionalExtensions;
using NotificationService.Entities;
using NotificationService.Entities.ValueObjects;
using NotificationService.SharedKernel;

namespace NotificationService.Services.Senders;

public interface INotificationSender
{
    Task<UnitResult<Error>> SendAsync(MessageData message, UserNotificationSettings userNotificationSetting,
        CancellationToken cancellationToken);

    bool CanSend(UserNotificationSettings userNotificationSetting, CancellationToken cancellationToken);
}