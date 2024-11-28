using NotificationService.BackgroundServices.Channels;
using NotificationService.Entities;

namespace NotificationService.BackgroundServices.Factories;

public class NotificationSettingsFactory
{
    private readonly IEnumerable<INotificationSender> _senders;

    public NotificationSettingsFactory(IEnumerable<INotificationSender> senders)
    {
        _senders = senders.ToList();
    }

    public IEnumerable<INotificationSender> GetSenders(NotificationSettings notificationSetting
        , CancellationToken cancellationToken)
    {
        var resultSenders = new List<INotificationSender>();

        if (notificationSetting.SendEmail)
        {
            var sender = _senders.OfType<EmailNotificationChannel>().FirstOrDefault();
            if (sender is not null && sender.CanSend(notificationSetting, cancellationToken))
            {
                resultSenders.Add(sender);
            }
        }

        if (notificationSetting.SendTelegram)
        {
            var sender = _senders.OfType<TelegramNotificationChannel>().FirstOrDefault();
            if (sender is not null && sender.CanSend(notificationSetting, cancellationToken))
            {
                resultSenders.Add(sender);
            }
        }

        if (notificationSetting.SendWeb)
        {
            var sender = _senders.OfType<WebNotificationChannel>().FirstOrDefault();
            if (sender is not null && sender.CanSend(notificationSetting, cancellationToken))
            {
                resultSenders.Add(sender);
            }
        }

        return resultSenders;
    }
}