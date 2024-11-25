using FluentValidation;
using SachkovTech.Core.Validation;
using SachkovTech.SharedKernel;

namespace SachkovTech.Issues.Application.Features.Lessons.Queries.GetLessonWithPagination;

public class GetLessonsWithPaginationValidator : AbstractValidator<GetLessonsWithPaginationQuery>
{
    public GetLessonsWithPaginationValidator()
    {
        RuleFor(v => v.Page)
            .GreaterThanOrEqualTo(1)
            .WithError(Errors.General.ValueIsInvalid("Page"));

        RuleFor(v => v.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithError(Errors.General.ValueIsInvalid("PageSize"));
    }
}
