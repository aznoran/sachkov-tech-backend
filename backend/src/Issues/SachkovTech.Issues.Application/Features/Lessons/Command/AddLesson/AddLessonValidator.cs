using FluentValidation;
using SachkovTech.Core.Validation;
using SachkovTech.Issues.Domain.Module.ValueObjects;
using SachkovTech.SharedKernel.ValueObjects;

namespace SachkovTech.Issues.Application.Features.Lessons.Command.AddLesson;

public class AddLessonValidator : AbstractValidator<AddLessonCommand>
{
    public AddLessonValidator()
    {
        RuleFor(a => a.Title)
            .MustBeValueObject(Title.Create);

        RuleFor(a => a.Description)
            .MustBeValueObject(Description.Create);

        RuleFor(a => a.Experience)
            .MustBeValueObject(Experience.Create);
    }
}
