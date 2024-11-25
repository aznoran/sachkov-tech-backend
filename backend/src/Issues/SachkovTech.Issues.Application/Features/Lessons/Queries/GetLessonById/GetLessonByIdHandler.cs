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
        var lesson = await context.Lessons
            .Include(l => l.Tags)
            .Include(l => l.Issues)
            .FirstOrDefaultAsync(l => l.Id == query.LessonId, cancellationToken);

        if (lesson is null)
            return Errors.General.NotFound(query.LessonId, "lesson").ToErrorList();

        //TODO: Реализовать получение Tag и Issue
        var lessonResponse = new LessonResponse
        {
            Id = lesson.Id,
            ModuleId = lesson.ModuleId,
            Title = lesson.Title,
            Description = lesson.Description,
            Experience = lesson.Experience,
            VideoId = lesson.VideoId,
            // TODO: сделать получение URL
            // VideoUrl = url,
            PreviewId = lesson.PreviewId,
            // TODO: сделать получение URL
            // PreviewUrl = url,
            Tags = lesson.Tags
                ?.Select(tag => new TagResponse
                {
                    Id = tag,
                    Name = "tag.Name"
                })
                .ToArray() ?? [],
            Issues = lesson.Issues
                ?.Select(issue => new IssueResponse
                {
                    Id = issue,
                    ModuleId = Guid.NewGuid(),
                    LessonId = Guid.NewGuid(),
                    Title = "issue.Title",
                    Description = "issue.Description",
                    Position = 1
                })
                .ToArray() ?? []
        };

        return lessonResponse;
    }
}