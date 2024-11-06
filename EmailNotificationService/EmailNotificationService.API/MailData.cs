namespace EmailNotificationService.API;

public class MailData
{
    // Recievers
    public List<string> To { get; set; } = [];

    // Content
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}
