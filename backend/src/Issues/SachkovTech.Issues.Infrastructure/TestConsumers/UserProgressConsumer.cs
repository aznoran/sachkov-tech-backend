using MassTransit;
using Microsoft.Extensions.Logging;
using SachkovTech.Issues.Contracts.Messaging;

namespace SachkovTech.Issues.Infrastructure.TestConsumers;

public class UserProgressConsumer : IConsumer<IssueSentOnReviewEvent>
{
    private readonly ILogger<UserProgressConsumer> _logger;

    public UserProgressConsumer(ILogger<UserProgressConsumer> logger)
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

public class UserProgressFaultConsumer : IConsumer<Fault<IssueSentOnReviewEvent>>
{
    private readonly ILogger<UserProgressFaultConsumer> _logger;

    public UserProgressFaultConsumer(ILogger<UserProgressFaultConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<Fault<IssueSentOnReviewEvent>> context)
    {
        // process event
        // отправить уведомление

        var attempt = context.GetRetryAttempt().ToString();

        _logger.LogInformation(attempt);

        return Task.CompletedTask;
    }
}

public class UserProgressConsumerDefinition : ConsumerDefinition<UserProgressConsumer>
{
    public UserProgressConsumerDefinition()
    {
        EndpointName = "event-queue";
    }

    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<UserProgressConsumer> consumerConfigurator,
        IRegistrationContext context)
    {
        endpointConfigurator.UseMessageRetry(x => x.Interval(3, TimeSpan.FromSeconds(3)));
    }
}