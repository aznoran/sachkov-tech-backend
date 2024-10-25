namespace EmailNotificationService.API
{
    public class MailData
    {
        // Reciever
        public List<string> To { get; init; } = [];

        // Content
        public string Subject { get; init; } = string.Empty;
        public string Body { get; init; } = string.Empty;

        public MailData(
                IEnumerable<string> to,
                string subject,
                string body)
        {
            // Reciever
            To = to.ToList();

            // Content
            Subject = subject;
            Body = body;
        }

    }
}
