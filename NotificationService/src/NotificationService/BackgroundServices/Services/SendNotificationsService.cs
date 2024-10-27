using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using NotificationService.BackgroundServices.Factories;
using NotificationService.Entities;
using NotificationService.HelperClasses;
using NotificationService.Infrastructure;

namespace NotificationService.BackgroundServices.Services;

public class SendNotificationsService
{
    private const int NOTIFICATIONS_TO_PROCESS = 10;
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<SendNotificationsService> _logger;
    private readonly NotificationSettingsFactory _notificationSettingsFactory;

    public SendNotificationsService(
        ApplicationDbContext dbContext,
        ILogger<SendNotificationsService> logger,
        NotificationSettingsFactory notificationSettingsFactory)
    {
        _dbContext = dbContext;
        _logger = logger;
        _notificationSettingsFactory = notificationSettingsFactory;
    }

    public async Task Process(CancellationToken cancellationToken)
    {
        var notifications = await GetNotificationsWithPendingStatusAsync(cancellationToken);

        foreach (var notification in notifications)
        {
            notification.SetNotificationStatus(NotificationStatusEnum.Processing);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        await ProcessNotifications(notifications, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task ProcessNotifications(List<Notification> notifications,
        CancellationToken cancellationToken)
    {
        foreach (var notification in notifications)
        {
            var notificationSettings = await
                GetNotificationSettingsAsync(notification, cancellationToken);

            var res = await
                ProcessNotificationSettings(notification, notificationSettings, cancellationToken);

            if (res.IsFailure)
            {
                _logger.LogError("Notification with {Id} failed to send", notification.Id);

                notification.SendingNotificationFailed();
            }

            _logger.LogInformation("Notification with {Id} sent successful", notification.Id);

            notification.SendingNotificationSuccedeed();
        }
    }

    private async Task<UnitResult<Error>> ProcessNotificationSettings(Notification notification,
        IEnumerable<NotificationSettings> notificationSettings,
        CancellationToken cancellationToken)
    {
        foreach (var notificationSetting in notificationSettings)
        {
            var senders = _notificationSettingsFactory
                .GetSenders(notificationSetting);

            var processSendersRes =
                await ProcessSenders(senders, notification, notificationSetting, cancellationToken);

            if (processSendersRes.IsFailure)
            {
                return processSendersRes.Error;
            }
        }

        return UnitResult.Success<Error>();
    }

    private async Task<UnitResult<Error>> ProcessSenders(IEnumerable<INotificationSender?> senders,
        Notification notification,
        NotificationSettings notificationSetting,
        CancellationToken cancellationToken)
    {
        foreach (var sender in senders)
        {
            if (sender != null && sender.CanSend(notificationSetting, cancellationToken))
            {
                var sendingResult = await sender
                    .SendAsync(notification.Message, notificationSetting, cancellationToken);
                
                if (sendingResult.IsFailure)
                {
                    return sendingResult.Error;
                }
            }
        }

        return UnitResult.Success<Error>();
    }

    private async Task<List<Notification>> GetNotificationsWithPendingStatusAsync
        (CancellationToken cancellationToken)
    {
        return await _dbContext.Notifications
            .Where(n => n.Status == NotificationStatusEnum.Pending)
            .Take(NOTIFICATIONS_TO_PROCESS)
            .ToListAsync(cancellationToken: cancellationToken);
    }

    private async Task<IEnumerable<NotificationSettings>> GetNotificationSettingsAsync
        (Notification notification, CancellationToken cancellationToken)
    {
        return await _dbContext.NotificationSettings
            .Where(nt => notification.UserIds.Contains(nt.UserId))
            .ToListAsync(cancellationToken);
    }
}