using FluentValidation;
using SachkovTech.Core.Validation;
using SachkovTech.SharedKernel.ValueObjects;

namespace SachkovTech.Issues.Application.Features.IssueSolving.Commands.SendOnReview;

public class SendOnReviewValidator : AbstractValidator<SendOnReviewCommand>
{
    public SendOnReviewValidator()
    {
        RuleFor(s => s.PullRequestUrl).MustBeValueObject(PullRequestUrl.Create);
    }
}