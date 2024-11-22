using AutoFixture;
using SachkovTech.Issues.Application.Features.Lessons.Command.AddLesson;

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
}