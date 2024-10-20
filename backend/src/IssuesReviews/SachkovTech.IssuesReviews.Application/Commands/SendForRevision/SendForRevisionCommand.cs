using SachkovTech.Core.Abstractions;

namespace SachkovTech.IssuesReviews.Application.Commands.SendForRevision;

public record SendForRevisionCommand(
    Guid IssueReviewId) : ICommand;