using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using SachkovTech.Core.Abstractions;
using SachkovTech.Core.Dtos;
using SachkovTech.Issues.Application.Interfaces;
using SachkovTech.SharedKernel;

namespace SachkovTech.Issues.Application.Features.Lessons.Queries.GetLessonById;

public class GetLessonByIdHandler(IReadDbContext context)
    : IQueryHandlerWithResult<LessonDto, GetLessonByIdQuery>
{
    public async Task<Result<LessonDto, ErrorList>> Handle(
        GetLessonByIdQuery query, CancellationToken cancellationToken = default)
    {
        var lesson = await context.Lessons.FirstOrDefaultAsync(l => l.Id == query.LessonId, cancellationToken);
        if (lesson is null)
            return Errors.General.NotFound(query.LessonId, "lesson").ToErrorList();

        return lesson;
    }
}