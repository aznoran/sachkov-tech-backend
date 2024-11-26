using MediatR;
using SachkovTech.Issues.Application.Interfaces;
using SachkovTech.Issues.Domain.IssueSolving.Events;

namespace SachkovTech.Issues.Application.EventHandlers.UserIssueSentOnReviewEventHandlers;

public class SendIntegrationEvent : INotificationHandler<UserIssueSentOnReviewEvent>
{
    private readonly IOutboxRepository _outboxRepository;
    public SendIntegrationEvent(IOutboxRepository outboxRepository)
    {
        _outboxRepository = outboxRepository;
    }

    public async Task Handle(UserIssueSentOnReviewEvent domainEvent, CancellationToken cancellationToken)
    {
        await _outboxRepository.Add(
            new IntegrationEvents.UserIssueSentOnReviewEvent(domainEvent.UserIssueId.Value),
            cancellationToken);
    }
}