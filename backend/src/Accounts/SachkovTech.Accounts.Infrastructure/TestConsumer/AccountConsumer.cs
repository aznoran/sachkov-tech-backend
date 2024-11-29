using MassTransit;
using SachkovTech.Issues.Contracts.Messaging;

namespace SachkovTech.Accounts.Infrastructure.TestConsumer;

public class AccountConsumer : IConsumer<IssueSentOnReviewEvent>
{
    public Task Consume(ConsumeContext<IssueSentOnReviewEvent> context)
    {
        return Task.CompletedTask;
    }
}