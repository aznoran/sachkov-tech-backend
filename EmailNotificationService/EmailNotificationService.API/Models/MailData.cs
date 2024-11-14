namespace EmailNotificationService.API.Models;

public class MailData
{

    public MailData(IEnumerable<string> recievers, string subject, string body)
    {
        To = recievers.ToList();
        Subject = subject;
        Body = body;
    }

    // Recievers
    public List<string> To { get; set; } = [];

    // Content
    public string Subject { get; } = string.Empty;
    public string Body { get; } = string.Empty;
}
