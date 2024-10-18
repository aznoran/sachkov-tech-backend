namespace NotificationService.Entities;

public class NotificationSettings
{
    public Guid Id { get; init; }

    public Guid UserId { get; init; }

    public bool Email { get; private set; } = true;

    public bool Telegram { get; private set; }

    public bool Web { get; private set; } = true;

    public void UpdatePreferences(
        bool? email = null,
        bool? telegram = null,
        bool? web = null)
    {
        Email = email ?? Email;
        Telegram = telegram ?? Telegram;
        Web = web ?? Web;
    }

}