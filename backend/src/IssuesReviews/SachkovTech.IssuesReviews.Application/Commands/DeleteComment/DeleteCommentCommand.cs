using SachkovTech.Core.Abstractions;

namespace SachkovTech.IssuesReviews.Application.Commands.DeleteComment;

public record DeleteCommentCommand(
    Guid IssueReviewId,
    Guid UserId,
    Guid CommentId) : ICommand;