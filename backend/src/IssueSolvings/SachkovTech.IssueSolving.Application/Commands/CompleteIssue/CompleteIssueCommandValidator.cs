using FluentValidation;
using SachkovTech.Core.Validation;
using SachkovTech.SharedKernel;

namespace SachkovTech.IssueSolving.Application.Commands.CompleteIssue;

public class CompleteIssueCommandValidator : AbstractValidator<CompleteIssueCommand>
{
    public CompleteIssueCommandValidator()
    {
        RuleFor(c => c.UserIssueId)
            .NotEmpty().WithError(Errors.General.ValueIsInvalid("id"));
    }
}
