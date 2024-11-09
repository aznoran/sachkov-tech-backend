using CSharpFunctionalExtensions;
using FluentValidation;
using SachkovTech.Core.Abstractions;
using SachkovTech.Core.Dtos;
using SachkovTech.Core.Extensions;
using SachkovTech.Core.Models;
using SachkovTech.Issues.Application.Interfaces;
using SachkovTech.SharedKernel;

namespace SachkovTech.Issues.Application.Features.Lessons.Queries.GetLessonWithPagination;

public class GetLessonsWithPaginationHandler(
    IValidator<GetLessonsWithPaginationValidatorQuery> validator,
    IReadDbContext context)
    : IQueryHandlerWithResult<PagedList<LessonDto>, GetLessonsWithPaginationValidatorQuery>
{
    public async Task<Result<PagedList<LessonDto>, ErrorList>> Handle(
        GetLessonsWithPaginationValidatorQuery query, CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(query, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToList();

        var lessonsQuery = context.Lessons;

        return  await lessonsQuery.ToPagedList(query.Page, query.PageSize, cancellationToken);
    }
}