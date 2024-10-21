using FluentValidation;
using SachkovTech.Core.Validation;
using SachkovTech.SharedKernel;

namespace SachkovTech.IssueSolving.Application.Commands.SendForRevision;

public class SendForRevisionCommandValidator : AbstractValidator<SendForRevisionCommand>
{
    public SendForRevisionCommandValidator()
    {
        RuleFor(c => c.UserIssueId)
            .NotEmpty().WithError(Errors.General.ValueIsInvalid("id"));
    }
}
