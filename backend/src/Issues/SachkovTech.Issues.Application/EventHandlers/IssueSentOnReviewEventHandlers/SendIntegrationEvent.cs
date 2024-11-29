using MassTransit;
using MassTransit.DependencyInjection;
using MediatR;
using SachkovTech.Issues.Application.Interfaces;
using SachkovTech.Issues.Domain.IssueSolving.Events;

namespace SachkovTech.Issues.Application.EventHandlers.IssueSentOnReviewEventHandlers;

public class SendIntegrationEvent : INotificationHandler<IssueSentOnReviewEvent>
{
    private readonly IOutboxRepository _outboxRepository;
    private readonly IPublishEndpoint _publishEndpoint;
    public SendIntegrationEvent(IOutboxRepository outboxRepository, Bind<IIssueMessageBus, IPublishEndpoint> publishEndpoint)
    {
        _outboxRepository = outboxRepository;
        _publishEndpoint = publishEndpoint.Value;
    }

    public async Task Handle(IssueSentOnReviewEvent domainEvent, CancellationToken cancellationToken)
    {
        await _publishEndpoint.Publish(new Contracts.Messaging.IssueSentOnReviewEvent(
            domainEvent.UserId,
            domainEvent.UserIssueId.Value,
            Guid.Empty), cancellationToken);


        // await _outboxRepository.Add(
        //     new Contracts.Messaging.IssueSentOnReviewEvent(domainEvent.UserIssueId.Value),
        //     cancellationToken);
    }
}