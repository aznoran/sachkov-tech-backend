using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using SachkovTech.Core.Abstractions;
using SachkovTech.Issues.Application.DataModels;
using SachkovTech.Issues.Application.Interfaces;
using SachkovTech.Issues.Contracts.Responses;
using SachkovTech.SharedKernel;

namespace SachkovTech.Issues.Application.Features.Lessons.Queries.GetLessonById;

public class GetLessonByIdHandler(IReadDbContext context)
    : IQueryHandlerWithResult<LessonResponse, GetLessonByIdQuery>
{
    public async Task<Result<LessonResponse, ErrorList>> Handle(
        GetLessonByIdQuery query, CancellationToken cancellationToken = default)
    {
        var lesson = await context.Lessons.FirstOrDefaultAsync(l => l.Id == query.LessonId, cancellationToken);
        if (lesson is null)
            return Errors.General.NotFound(query.LessonId, "lesson").ToErrorList();

        var lessonResponse = new LessonResponse
        {
            Id = lesson.Id,
            ModuleId = lesson.ModuleId,
            Title = lesson.Title,
            Description = lesson.Description,
            Experience = lesson.Experience,
            VideoId = lesson.VideoId,
            //TODO: сделать получение url
            // VideoUrl = url
            PreviewId = lesson.PreviewId,
            //TODO: сделать получение url
            // PreviewUrl = url
            Tags = lesson.Tags,
            Issues = lesson.Issues
        };
        
        return lessonResponse;
    }
}