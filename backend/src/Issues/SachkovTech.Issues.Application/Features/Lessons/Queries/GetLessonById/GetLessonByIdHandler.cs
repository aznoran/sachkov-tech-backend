using CSharpFunctionalExtensions;
using FileService.Communication;
using FileService.Contracts;
using Microsoft.EntityFrameworkCore;
using SachkovTech.Core.Abstractions;
using SachkovTech.Core.Dtos;
using SachkovTech.Issues.Application.Interfaces;
using SachkovTech.SharedKernel;

namespace SachkovTech.Issues.Application.Features.Lessons.Queries.GetLessonById;

public class GetLessonByIdHandler(
    IReadDbContext context,
    IFileService fileService) : IQueryHandlerWithResult<LessonResponse, GetLessonByIdQuery>
{
    public async Task<Result<LessonResponse, ErrorList>> Handle(
        GetLessonByIdQuery query, CancellationToken cancellationToken = default)
    {
        var lesson = await context.Lessons.FirstOrDefaultAsync(l => l.Id == query.LessonId, cancellationToken);
        if (lesson is null)
            return Errors.General.NotFound(query.LessonId, "lesson").ToErrorList();

        var fileServiceRequest = new GetFilesPresignedUrlsRequest([lesson.VideoId, lesson.PreviewId]);

        var urlsResult = await fileService.GetFilesPresignedUrls(fileServiceRequest, cancellationToken);
        if (urlsResult.IsFailure)
            return Errors.General.NotFound().ToErrorList();
        
        var urls = urlsResult.Value.ToDictionary(v => v.FileId, u => u.PresignedUrl);
        
        var lessonResponse = ToLessonResponse(lesson, urls);
       
        return lessonResponse;
    }

    private LessonResponse ToLessonResponse(LessonDto lesson, Dictionary<Guid, string> urls) =>
        new(lesson.Id,
            lesson.ModuleId,
            lesson.Title,
            lesson.Description,
            lesson.Experience,
            lesson.VideoId,
            urls[lesson.VideoId],
            lesson.PreviewId,
            urls[lesson.PreviewId],
            lesson.Tags,
            lesson.Issues);
}
