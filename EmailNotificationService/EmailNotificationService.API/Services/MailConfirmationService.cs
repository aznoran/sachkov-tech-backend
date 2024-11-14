using CSharpFunctionalExtensions;
using EmailNotificationService.API.Features;
using EmailNotificationService.API.Models;
using EmailNotificationService.API.Requests;

namespace EmailNotificationService.API.Services;

public class MailConfirmationService
{
    private readonly MailSender _mailSender;

    public MailConfirmationService(MailSender mailSender)
    {
        _mailSender = mailSender;
    }

    public async Task<UnitResult<string>> Execute(MailConfirmationRequest request)
    {
        var template = File.ReadAllText(
            Path.Combine(
                Directory.GetCurrentDirectory(), 
                "wwwroot", 
                "Templates", 
                "EmailConfirmation.html"));

        var mailBody = template
            .Replace("{{FullName}}", request.FullName)
            .Replace("{{ConfirmationLink}}", request.ConfirmationLink);


        var mailData = new MailData([request.Email], "Confirm your e-mail address", mailBody);

        var sendResult = await _mailSender.Send(mailData);
        if (sendResult.IsFailure)
        {
            return sendResult.Error;
        }

        return UnitResult.Success<string>();
    }
}
