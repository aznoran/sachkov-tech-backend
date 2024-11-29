using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Issues.Domain.IssueSolving.Events;

public record IssueSentOnReviewEvent(
    UserIssueId UserIssueId,
    Guid UserId,
    PullRequestUrl PullRequestUrl) : IDomainEvent;