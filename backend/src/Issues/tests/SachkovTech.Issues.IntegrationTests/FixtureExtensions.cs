using AutoFixture;
using SachkovTech.Issues.Application.Features.Issue.Commands.AddIssue;
using SachkovTech.Issues.Application.Features.Issue.Commands.DeleteIssue;
using SachkovTech.Issues.Application.Features.Issue.Commands.RestoreIssue;
using SachkovTech.Issues.Application.Features.Issue.Commands.UpdateIssueMainInfo;
using SachkovTech.Issues.Application.Features.Lessons.Command.AddLesson;
using SachkovTech.Issues.Application.Features.Lessons.Queries.GetLessonById;
using SachkovTech.Issues.Application.Features.Lessons.Queries.GetLessonsWithPagination;
using SachkovTech.Issues.Application.Features.Modules.Commands.Create;
using SachkovTech.Issues.Application.Features.Modules.Commands.Delete;

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
    
    public static AddIssueCommand CreateAddIssueCommand(
        this IFixture fixture,
        Guid moduleId,
        Guid lessonId)
    {
        return fixture.Build<AddIssueCommand>()
            .With(c => c.LessonId, lessonId)
            .With(c => c.ModuleId, moduleId)
            .With(c => c.Experience, 5)
            .Create();
    }
    
    public static UpdateIssueMainInfoCommand CreateUpdateIssueMainInfoCommand(
        this IFixture fixture,
        Guid issueId,
        Guid lessonId,
        Guid moduleId)
    {
        return fixture.Build<UpdateIssueMainInfoCommand>()
            .With(c => c.IssueId, issueId)
            .With(c => c.LessonId, lessonId)
            .With(c => c.ModuleId, moduleId)
            .With(c => c.Experience, 5)
            .Create();
    }
    
    public static DeleteIssueCommand CreateDeleteIssueCommand(
        this IFixture fixture,
        Guid issueId)
    {
        return fixture.Build<DeleteIssueCommand>()
            .With(c => c.IssueId, issueId)
            .Create();
    }
    
    public static RestoreIssueCommand CreateRestoreIssueCommand(
        this IFixture fixture,
        Guid issueId)
    {
        return fixture.Build<RestoreIssueCommand>()
            .With(c => c.IssueId, issueId)
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

    public static CreateModuleCommand CreateCreateModuleCommand(this IFixture fixture)
    {
        return fixture.Create<CreateModuleCommand>();
    }
    
    public static DeleteModuleCommand CreateDeleteModuleCommand(this IFixture fixture, Guid moduleId)
    {
        return fixture.Build<DeleteModuleCommand>()
            .With(c => c.ModuleId, moduleId)
            .Create();
    }
}
