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

        return Task.CompletedTask;
    }
}