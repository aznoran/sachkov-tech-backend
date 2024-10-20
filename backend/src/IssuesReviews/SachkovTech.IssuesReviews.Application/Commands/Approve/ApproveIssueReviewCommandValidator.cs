using FluentValidation;
using SachkovTech.Core.Validation;
using SachkovTech.IssuesReviews.Application.Commands.SendForRevision;
using SachkovTech.SharedKernel;

namespace SachkovTech.IssuesReviews.Application.Commands.Approve;

public class ApproveIssueReviewCommandValidator : AbstractValidator<ApproveIssueReviewCommand>
{
    public ApproveIssueReviewCommandValidator()
    {
        RuleFor(c => c.IssueReviewId)
            .NotEmpty().WithError(Errors.General.ValueIsInvalid("id"));
    }
}
