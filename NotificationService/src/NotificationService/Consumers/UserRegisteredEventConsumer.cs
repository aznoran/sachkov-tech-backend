using EmailNotification.Contacts;
using MassTransit;
using SachkovTech.Accounts.Contracts.Messaging;
using SachkovTech.Accounts.Contracts.Responses;

namespace NotificationService.Consumers;

public class UserRegisteredEventConsumer : IConsumer<UserRegisteredEvent>
{
    public async Task Consume(ConsumeContext<UserRegisteredEvent> context)
    {
        // достать информацию о пользователе из Account service
        
        // создать комманду для отправки уведомления на почту, телеграм или веб
        
        var userResponse = new UserResponse(Guid.NewGuid(), "", "", "", "sachkov@gmail.com", DateTime.Now, [], null, null, null, []);

        var sendEmailCommand = new SendEmailCommand(
            userResponse.Email,
            "Hello! You have successfully registered!",
            "registration",
            new
            {
                ConfirmationLink = "link",
            }
        );

        await context.Publish(sendEmailCommand, context.CancellationToken);
    }
}