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

    public IEnumerable<INotificationSender?> GetSenders(NotificationSettings notificationSetting)
    {
        var resultSenders = new List<INotificationSender?>();

        if (notificationSetting.SendEmail)
        {
            resultSenders.Add(_senders.OfType<EmailNotificationChannel>().FirstOrDefault());
        }

        if (notificationSetting.SendTelegram)
        {
            resultSenders.Add(_senders.OfType<TelegramNotificationChannel>().FirstOrDefault());
        }

        if (notificationSetting.SendWeb)
        {
            resultSenders.Add(_senders.OfType<WebNotificationChannel>().FirstOrDefault());
        }

        return resultSenders;
    }
}