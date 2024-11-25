using CSharpFunctionalExtensions;
using FileService.Contracts;
using FluentValidation;
using SachkovTech.Core.Abstractions;
using SachkovTech.Core.Dtos;
using SachkovTech.Core.Extensions;
using SachkovTech.Core.Models;
using SachkovTech.Issues.Application.Interfaces;
using SachkovTech.SharedKernel;
using FileService.Communication;

namespace SachkovTech.Issues.Application.Features.Lessons.Queries.GetLessonWithPagination;

public class GetLessonsWithPaginationHandler(
    IValidator<GetLessonsWithPaginationQuery> validator,
    IFileService fileHttpClient,
    IReadDbContext context)
    : IQueryHandlerWithResult<PagedList<LessonResponse>, GetLessonsWithPaginationQuery>
{
    public async Task<Result<PagedList<LessonResponse>, ErrorList>> Handle(
        GetLessonsWithPaginationQuery query, CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(query, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToList();
        var lessonsQuery = context.Lessons;
        var lessonsPagedList = await lessonsQuery.ToPagedList(query.Page, query.PageSize, cancellationToken);

        var videoIds = lessonsPagedList.Items
            .SelectMany(l => new[] { l.VideoId, l.PreviewId })
            .ToList();
        var videoRequest = new GetFilesPresignedUrlsRequest(videoIds);

        var videoUrlsResult = await fileHttpClient.GetFilesPresignedUrls(videoRequest, cancellationToken);
        if (videoUrlsResult.IsFailure)
            return Errors.General.NotFound().ToErrorList();

        return ConvertToLessonResponses(videoUrlsResult.Value, lessonsPagedList);
    }

    private PagedList<LessonResponse> ConvertToLessonResponses(
        IReadOnlyList<FileResponse> videoUrlsResult, PagedList<LessonDto> lessonsPagedList)
    {
        var urls = videoUrlsResult.ToDictionary(v => v.FileId, u => u.PresignedUrl);
        var lessons = lessonsPagedList.Items
            .Select(lessonDto => new LessonResponse(lessonDto.Id,
                lessonDto.ModuleId,
                lessonDto.Title,
                lessonDto.Description,
                lessonDto.Experience,
                lessonDto.VideoId,
                urls[lessonDto.VideoId],
                lessonDto.PreviewId,
                urls[lessonDto.PreviewId],
                lessonDto.Tags, lessonDto.Issues))
            .ToList();
        return new PagedList<LessonResponse>
        {
            Items = lessons.AsReadOnly(),
            TotalCount = lessonsPagedList.TotalCount,
            PageSize = lessonsPagedList.PageSize,
            Page = lessonsPagedList.Page
        };
    }
}
