using MassTransit;
using Microsoft.Extensions.Logging;
using SachkovTech.Issues.Contracts.Messaging;

namespace SachkovTech.Issues.Infrastructure.TestConsumers;

public class NotificationConsumer : IConsumer<IssueSentOnReviewEvent>
{
    private readonly ILogger<NotificationConsumer> _logger;

    public NotificationConsumer(ILogger<NotificationConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<IssueSentOnReviewEvent> context)
    {
        // process event
        // отправить уведомление

        _logger.LogInformation(context.Message.UserId.ToString());

        return Task.CompletedTask;
    }
}