using FluentValidation;
using SachkovTech.Core.Validation;
using SachkovTech.IssuesReviews.Application.Commands.AddComment;
using SachkovTech.SharedKernel;

namespace SachkovTech.IssuesReviews.Application.Commands.DeleteComment;

public class DeleteCommentCommandValidator : AbstractValidator<DeleteCommentCommand>
{
    public DeleteCommentCommandValidator()
    {
        RuleFor(c => c.IssueReviewId)
            .NotEmpty().WithError(Errors.General.ValueIsInvalid("id"));
        RuleFor(c => c.CommentId)
            .NotEmpty().WithError(Errors.General.ValueIsInvalid("comment_id"));
    }
}