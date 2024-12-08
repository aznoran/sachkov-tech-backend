using EmailNotification.Contacts;
using MassTransit;
using SachkovTech.Accounts.Communication;
using SachkovTech.Accounts.Contracts.Messaging;

namespace NotificationService.Consumers;

public class UserRegisteredEventConsumer : IConsumer<UserRegisteredEvent>
{
    private readonly AccountHttpClient _httpClient;

    public UserRegisteredEventConsumer(AccountHttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task Consume(ConsumeContext<UserRegisteredEvent> context)
    {
        if (context.Message.UserId == Guid.Empty) return;

        var result = await _httpClient.GetConfirmationLink(context.Message.UserId);

        if (result.IsFailure)
        {
            throw new Exception(result.Error);
        }
        
        var sendEmailCommand = new SendEmailCommand(
            result.Value.Email,
            "Подтверждение почты!",
            "registration-confirmation",
            new
            {
                ConfirmationLink = result.Value.ConfirmationLink
            }
        );

        await context.Publish(sendEmailCommand, context.CancellationToken);
    }
}

