using FluentValidation;
using SachkovTech.Core.Validation;
using SachkovTech.SharedKernel;

namespace SachkovTech.IssuesReviews.Application.Commands.SendForRevision;

public class SendForRevisionCommandValidator : AbstractValidator<SendForRevisionCommand>
{
    public SendForRevisionCommandValidator()
    {
        RuleFor(c => c.IssueReviewId)
            .NotEmpty().WithError(Errors.General.ValueIsInvalid("id"));
    }
}
