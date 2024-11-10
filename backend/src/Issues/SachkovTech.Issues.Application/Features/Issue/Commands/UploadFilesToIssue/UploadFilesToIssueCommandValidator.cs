using FluentValidation;
using SachkovTech.Core.Validation;
using SachkovTech.SharedKernel;

namespace SachkovTech.Issues.Application.Features.Issue.Commands.UploadFilesToIssue;

public class UploadFilesToIssueCommandValidator : AbstractValidator<UploadFilesToIssueCommand>
{
    public UploadFilesToIssueCommandValidator()
    {
        RuleFor(u => u.IssueId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}