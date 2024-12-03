using CSharpFunctionalExtensions;
using NotificationService.Entities;
using NotificationService.Entities.ValueObjects;
using NotificationService.SharedKernel;

namespace NotificationService.Services.Senders;

public class WebNotificationSender : INotificationSender
{
    public Task<UnitResult<Error>> SendAsync(MessageData message, UserNotificationSettings userNotificationSetting, CancellationToken cancellationToken)
    {
        return Task.FromResult(UnitResult.Success<Error>());
    }

    public bool CanSend(UserNotificationSettings userNotificationSetting, CancellationToken cancellationToken)
    {
        return true;
    }
}