﻿using NotificationService.Entities;
using NotificationService.Services.Senders;

namespace NotificationService.Services.Factories;

public class NotificationSettingsFactory
{
    private readonly IEnumerable<INotificationSender> _senders;

    public NotificationSettingsFactory(IEnumerable<INotificationSender> senders)
    {
        _senders = senders.ToList();
    }

    public IEnumerable<INotificationSender> GetSenders(UserNotificationSettings userNotificationSetting, CancellationToken cancellationToken)
        => _senders.Where(sender => sender.CanSend(userNotificationSetting, cancellationToken));
}