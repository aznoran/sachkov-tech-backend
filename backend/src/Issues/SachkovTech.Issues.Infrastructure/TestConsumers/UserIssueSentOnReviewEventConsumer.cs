using MassTransit;
using Microsoft.Extensions.Logging;
using SachkovTech.Issues.Contracts.Messaging;

namespace SachkovTech.Issues.Infrastructure.TestConsumers;

public class UserIssueSentOnReviewEventConsumer : IConsumer<UserIssueSentOnReviewEvent>
{
    private readonly ILogger<UserIssueSentOnReviewEventConsumer> _logger;
    public UserIssueSentOnReviewEventConsumer(ILogger<UserIssueSentOnReviewEventConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<UserIssueSentOnReviewEvent> context)
    {
        _logger.LogInformation(context.Message.UserIssueId.ToString());

        return Task.CompletedTask;
    }
}