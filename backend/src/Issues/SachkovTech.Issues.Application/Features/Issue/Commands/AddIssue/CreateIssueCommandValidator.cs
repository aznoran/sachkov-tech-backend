using FluentValidation;
using SachkovTech.Core.Validation;
using SachkovTech.SharedKernel.ValueObjects;

namespace SachkovTech.Issues.Application.Features.Issue.Commands.AddIssue;

public class CreateIssueCommandValidator : AbstractValidator<CreateIssueCommand>
{
    public CreateIssueCommandValidator()
    {
        RuleFor(c => c.Title).MustBeValueObject(Title.Create);
        RuleFor(c => c.Description).MustBeValueObject(Description.Create);
    }
}