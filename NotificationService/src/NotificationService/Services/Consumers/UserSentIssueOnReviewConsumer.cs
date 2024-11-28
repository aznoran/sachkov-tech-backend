using System.Text.Json;
using MassTransit;
using NotificationService.Entities.ValueObjects;
using SachkovTech.Issues.Contracts.Messaging;

namespace NotificationService.Services.Consumers;

public class UserSentIssueOnReviewConsumer : IConsumer<UserSentIssueOnReviewEvent>
{
    private readonly ILogger<UserSentIssueOnReviewConsumer> _logger;
    public UserSentIssueOnReviewConsumer(ILogger<UserSentIssueOnReviewConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<UserSentIssueOnReviewEvent> context)
    {
        _logger.LogInformation(context.Message.UserIssueId.ToString());

        // обработка сообщения
        // собраться нужные данные для отправки уведомления
        // выбрать template
        // собрать данные, положить их в объект

        var obj = new
        {
            UserName = "ПользательX",
            PullRequest = "pull request"
        };

        var data = JsonSerializer.Serialize(obj);

        var messageData = new MessageData("Новое ревью", "Review", data);

        // отправить messageData в почтовый сервис

        return Task.CompletedTask;
    }
}