using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace EmailNotificationService.API;

public partial class MailSender
{
    private const string EMAIL_REGEX_PATTERN = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
    private const string INVALID_EMAIL_ERR = "Request doesn't contain any valid reciever's adress. Aborting sending.";

    private readonly MailOptions _options;
    private readonly ILogger<MailSender> _logger;

    public MailSender(
        IOptions<MailOptions> options,
        ILogger<MailSender> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public async Task<UnitResult<string>> Send(MailData mailData) 
    {
        var mail = new MimeMessage();

        mail.From.Add(new MailboxAddress(_options.FromDisplayName, _options.From));

        foreach (var address in mailData.To)
        {
            if (EmailRegex().IsMatch(address))
            {
                MailboxAddress.TryParse(address, out var mailAddress);
                mail.To.Add(mailAddress!);
            }
            else 
            {
                _logger.LogError("Incorrect email address: {address}", address);
            }
        }

        if (mail.To.Count == 0)
        {
            _logger.LogError(INVALID_EMAIL_ERR);
            return INVALID_EMAIL_ERR;
        }
            
        var body = new BodyBuilder { HtmlBody = mailData.Body };

        mail.Body = body.ToMessageBody();
        mail.Subject = mailData.Subject;

        using var client = new SmtpClient();

        await client.ConnectAsync(_options.Host, _options.Port);
        await client.AuthenticateAsync(_options.UserName, _options.Password);
        await client.SendAsync(mail);

        foreach (var address in mail.To)
            _logger.LogInformation("Email succesfully sended to {to}", address);

        return UnitResult.Success<string>();
    }

    [GeneratedRegex(EMAIL_REGEX_PATTERN)]
    private static partial Regex EmailRegex();
}
