using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;

namespace EmailNotificationService.API;

public class MailSender
{
    private readonly MailOptions _options;
    private readonly ILogger<MailSender> _logger;
    public MailSender(
        IOptions<MailOptions> options,
        ILogger<MailSender> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public async Task<bool> Send(MailData mailData) 
    {
        try
        {
            var mail = new MimeMessage();

            mail.From.Add(new MailboxAddress(_options.FromDisplayName, _options.From));

            foreach (var address in mailData.To)
            {
                if (MailboxAddress.TryParse(address, out var mailAddress) == true)
                    mail.To.Add(mailAddress!);
            }

            var body = new BodyBuilder { HtmlBody = mailData.Body };

            mail.Body = body.ToMessageBody();
            mail.Subject = mailData.Subject;

            using var client = new SmtpClient();

            await client.ConnectAsync(_options.Host, _options.Port);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            await client.AuthenticateAsync(_options.UserName, _options.Password);
            await client.SendAsync(mail);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return false;
        }

        _logger.LogInformation("Email succesfully sended to {to}", mailData.To);

        return true;
    }
}
