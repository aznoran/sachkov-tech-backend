using SachkovTech.Core.Abstractions;

namespace SachkovTech.Issues.Application.Features.IssuesReviews.Commands.Create;

public record CreateIssueReviewCommand(Guid UserIssueId, Guid UserId, string PullRequestUrl) : ICommand;