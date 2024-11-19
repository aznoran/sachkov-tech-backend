using CSharpFunctionalExtensions;
using EmailNotificationService.API.Handlebars;
using EmailNotificationService.API.Models;
using EmailNotificationService.API.Requests;
using EmailNotificationService.API.Services;

namespace EmailNotificationService.API.Features;

public class SendEmailConfirmation
{
    private readonly MailSenderService _mailSender;
    private readonly HandlebarsTemplateService _handlebarTemplateService;

    public SendEmailConfirmation(
        MailSenderService mailSender, HandlebarsTemplateService handlebarTemplateService)
    {
        _mailSender = mailSender;
        _handlebarTemplateService = handlebarTemplateService;
    }

    public async Task<UnitResult<string>> Execute(MailConfirmationRequest request)
    {
        var userDetails = new EmailConfirmationDetails(request.FullName, request.ConfirmationLink);

        var mailBody = _handlebarTemplateService.Process(userDetails);

        var mailData = new MailData([request.Email], "Confirm your e-mail address", mailBody);

        var sendResult = await _mailSender.Send(mailData);
        if (sendResult.IsFailure)
        {
            return sendResult.Error;
        }

        return UnitResult.Success<string>();
    }
}
