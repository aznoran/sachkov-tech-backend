using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Issues.Domain.IssuesReviews.Events;

public record IssueSentForRevisionEvent(UserIssueId UserIssueId) : IDomainEvent;