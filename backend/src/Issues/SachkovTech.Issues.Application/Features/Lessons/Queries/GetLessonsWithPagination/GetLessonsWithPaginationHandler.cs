using CSharpFunctionalExtensions;
using FluentValidation;
using SachkovTech.Core.Abstractions;
using SachkovTech.Core.Extensions;
using SachkovTech.Core.Models;
using SachkovTech.Issues.Application.Interfaces;
using SachkovTech.Issues.Contracts.Responses;
using SachkovTech.SharedKernel;

namespace SachkovTech.Issues.Application.Features.Lessons.Queries.GetLessonsWithPagination;

public class GetLessonsWithPaginationHandler(
    IValidator<GetLessonsWithPaginationValidatorQuery> validator,
    //FileHttpClient fileHttpClient,
    IReadDbContext context)
    : IQueryHandlerWithResult<PagedList<LessonResponse>, GetLessonsWithPaginationValidatorQuery>
{
    public async Task<Result<PagedList<LessonResponse>, ErrorList>> Handle(
        GetLessonsWithPaginationValidatorQuery query, CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(query, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToList();

        var lessonsQuery = context.Lessons;

        var lessonsPagedList = await lessonsQuery.ToPagedList(query.Page, query.PageSize, cancellationToken);

        // мы должны получить videoUrl из FileService
        // var videoIds = lessonsPagedList.Items.Select(l => l.VideoId);
        List<Guid> videoIds = [Guid.Parse("2572d9ad-a013-4645-be3e-b79dbfcd4c09")];

        //var videoUrlsResult = await fileHttpClient.GetFilesPresignedUrls(new GetFilesPresignedUrlsRequest(videoIds), cancellationToken);
        // if (videoUrlsResult.IsFailure)
        //     return Errors.General.NotFound().ToErrorList();

        var videoUrl = "videoUrl";

        return new PagedList<LessonResponse>
        {
            Page = lessonsPagedList.Page,
            PageSize = lessonsPagedList.PageSize,
            TotalCount = lessonsPagedList.TotalCount,
            Items = lessonsPagedList.Items.Select(dm => new LessonResponse
            {
                Id = dm.Id,
                ModuleId = dm.ModuleId,
                Title = dm.Title,
                Description = dm.Description,
                Experience = dm.Experience,
                VideoId = dm.VideoId,
                //TODO: сделать получение url
                // VideoUrl = url
                PreviewId = dm.PreviewId,
                //TODO: сделать получение url
                // PreviewUrl = url
                Tags = dm.Tags,
                Issues = dm.Issues
            }).ToList()
        };
    }
}