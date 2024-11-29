using MassTransit;

namespace SachkovTech.Issues.Infrastructure.TestConsumers;

public class NotificationConsumerDefinition : ConsumerDefinition<NotificationConsumer>
{
    public NotificationConsumerDefinition()
    {
        EndpointName = "event-queue";
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<NotificationConsumer> consumerConfigurator, IRegistrationContext context)
    {
        endpointConfigurator.UseMessageRetry(x => x.Interval(3, TimeSpan.FromSeconds(3)));
    }
}