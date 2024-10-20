using SachkovTech.Core.Abstractions;

namespace SachkovTech.IssuesReviews.Application.Commands.Approve;

public record ApproveIssueReviewCommand(
    Guid IssueReviewId) : ICommand;