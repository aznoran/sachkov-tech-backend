using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using NotificationService.Entities;
using NotificationService.HelperClasses;
using NotificationService.Infrastructure;
using NotificationService.Services.Factories;

namespace NotificationService.Services.Services;

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

        await HandleNotificationsAsync(notifications, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task HandleNotificationsAsync(List<Notification> notifications,
        CancellationToken cancellationToken)
    {
        foreach (var notification in notifications)
        {
            var notificationSettings = await
                GetNotificationSettingsAsync(notification, cancellationToken);

            var res = await
                ApplyNotificationSettingsAsync(notification, notificationSettings, cancellationToken);

            if (res.IsFailure)
            {
                _logger.LogError("Notification with {Id} failed to send", notification.Id);

                notification.SendingNotificationFailed();
            }

            _logger.LogInformation("Notification with {Id} sent successful", notification.Id);

            notification.SendingNotificationSuccedeed();
        }
    }

    private async Task<UnitResult<Error>> ApplyNotificationSettingsAsync(Notification notification,
        IEnumerable<NotificationSettings> notificationSettings,
        CancellationToken cancellationToken)
    {
        foreach (var notificationSetting in notificationSettings)
        {
            var notificationSenders = _notificationSettingsFactory.GetSenders(notificationSetting, cancellationToken);

            var processSendersRes = await HandleSendersForNotificationSettingsAsync(
                notificationSenders,
                notification,
                notificationSetting,
                cancellationToken);

            if (processSendersRes.IsFailure)
            {
                return processSendersRes.Error;
            }
        }

        return UnitResult.Success<Error>();
    }

    private async Task<UnitResult<Error>> HandleSendersForNotificationSettingsAsync(
        IEnumerable<INotificationSender> senders,
        Notification notification,
        NotificationSettings notificationSetting,
        CancellationToken cancellationToken)
    {
        var sendTasks = senders.Select(sender =>
            sender.SendAsync(notification.Message, notificationSetting, cancellationToken)).ToList();

        var sendingResults = await Task.WhenAll(sendTasks);

        var failureResult = sendingResults.FirstOrDefault(result => result.IsFailure);

        if (failureResult.IsFailure)
        {
            return failureResult.Error;
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