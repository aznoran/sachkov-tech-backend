using MediatR;
using SachkovTech.Issues.Application.Interfaces;
using SachkovTech.Issues.Domain.IssueSolving.Events;

namespace SachkovTech.Issues.Application.EventHandlers.UserSentIssueOnReviewEventHandlers;

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
            new Contracts.Messaging.UserSentIssueOnReviewEvent(domainEvent.UserIssueId.Value),
            cancellationToken);
    }
}