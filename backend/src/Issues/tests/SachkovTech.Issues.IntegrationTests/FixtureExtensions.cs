using AutoFixture;
using SachkovTech.Issues.Application.Features.Lessons.Command.AddLesson;
using SachkovTech.Issues.Application.Features.Lessons.Queries.GetLessonById;
using SachkovTech.Issues.Application.Features.Lessons.Queries.GetLessonWithPagination;

namespace SachkovTech.Issues.IntegrationTests;

public static class FixtureExtensions
{
    public static AddLessonCommand CreateAddLessonCommand(
        this IFixture fixture,
        Guid moduleId)
    {
        return fixture.Build<AddLessonCommand>()
            .With(c => c.ModuleId, moduleId)
            .With(c => c.FileName, "file.mp4")
            .With(c => c.ContentType, "video/mp4")
            .With(c => c.FileSize, 1024)
            .Create();
    }

    public static GetLessonByIdQuery CreateGetLessonByIdQuery(
        this IFixture fixture,
        Guid lessonId)
    {
        return fixture.Build<GetLessonByIdQuery>()
            .With(c => c.LessonId, lessonId)
            .Create();
    }

    public static GetLessonsWithPaginationQuery CreateGetLessonsWithPaginationQuery(
        this IFixture fixture,
        int page,
        int pageSize)
    {
        return fixture.Build<GetLessonsWithPaginationQuery>()
            .With(c => c.Page, page)
            .With(c => c.PageSize, pageSize)
            .Create();
    }
}
