using FluentValidation;

namespace SachkovTech.Issues.Application.Features.Module.Commands.Delete;

public class DeleteModuleCommandValidator : AbstractValidator<DeleteModuleCommand>
{
    public DeleteModuleCommandValidator()
    {
        RuleFor(d => d.ModuleId).NotEmpty();
    }
}