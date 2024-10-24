using Microsoft.EntityFrameworkCore;
using NotificationService.BackgroundServices.Channels;
using NotificationService.Entities;
using NotificationService.Infrastructure;

namespace NotificationService.BackgroundServices.Services;

public class SendNotificationsService
{
    private const int NOTIFICATIONS_TO_PROCESS = 10;
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<SendNotificationsService> _logger;
    private INotificationSender _notificationSender;

    public SendNotificationsService(
        ApplicationDbContext dbContext,
        ILogger<SendNotificationsService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    
    public async Task Process(CancellationToken cancellationToken)
    {
        var notifications = await GetNotificationsWithPendingStatusAsync(cancellationToken);

        foreach (var notification in notifications)
        {
            notification.SetNotificationStatus(NotificationStatusEnum.Processing);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
        
        foreach (var notification in notifications)
        {
            var notificationSettings = await 
                GetNotificationSettingsAsync(notification, cancellationToken);

            try
            {
                foreach (var notificationSetting in notificationSettings)
                {
                    if (notificationSetting.SendEmail)
                    {
                        _notificationSender = new EmailNotificationChannel();

                        await TryToSend(notification, notificationSetting, cancellationToken);
                    }

                    if (notificationSetting.SendTelegram)
                    {
                        _notificationSender = new TelegramNotificationChannel();
                        
                        await TryToSend(notification, notificationSetting, cancellationToken);
                    }

                    if (notificationSetting.SendWeb)
                    {
                        _notificationSender = new WebNotificationChannel();
                        
                        await TryToSend(notification, notificationSetting, cancellationToken);
                    }

                    _logger.LogInformation("Notification with {Id} sent successful", notification.Id);
                }
                notification.SendingNotificationSuccedeed();
            }
            catch
            {
                _logger.LogError("Notification with {Id} failed to send", notification.Id);
                notification.SendingNotificationFailed();
            }
        }
        
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task TryToSend(Notification notification,
        NotificationSettings notificationSettings,
        CancellationToken cancellationToken)
    {
        if(_notificationSender.CanSend(notificationSettings, cancellationToken))
            await _notificationSender.SendAsync(notification.Message, notificationSettings, cancellationToken);
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