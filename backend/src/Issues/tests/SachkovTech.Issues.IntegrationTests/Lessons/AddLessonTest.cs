using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SachkovTech.Issues.Application.Features.Lessons.Command.AddLesson;
using SachkovTech.Issues.Domain.Module;
using SachkovTech.Issues.Infrastructure.DbContexts;
using SachkovTech.SharedKernel.ValueObjects;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Issues.IntegrationTests.Lessons;

public class AddLessonTest : LessonsTestsBase
{
    public AddLessonTest(IntegrationTestsWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Add_lesson_to_database()
    {
        // act
        var cancellationToken = new CancellationTokenSource().Token;

        var moduleId = await SeedModuleToDatabase(WriteDbContext, cancellationToken);

        var command = Fixture.Build<AddLessonCommand>()
            .With(c => c.ModuleId, moduleId)
            .Create();

        var handler = Scope.ServiceProvider.GetRequiredService<AddLessonHandler>();

        // arrange
        var result = await handler.Handle(command, cancellationToken);

        //assert
        var lesson = await ReadDbContext.Lessons
            .FirstOrDefaultAsync(l => l.Id == result.Value, cancellationToken);

        result.IsSuccess.Should().BeTrue();
        lesson.Should().NotBeNull();
        lesson?.Title.Should().Be(command.Title);
    }

    [Fact]
    public async Task Cant_add_lesson_to_database()
    {
        // act
        var cancellationToken = new CancellationTokenSource().Token;
        var fixture = new Fixture();

        var command = fixture.Build<AddLessonCommand>()
            .With(c => c.ModuleId, Guid.Empty)
            .Create();

        var handler = Scope.ServiceProvider.GetRequiredService<AddLessonHandler>();

        // arrange
        var result = await handler.Handle(command, cancellationToken);

        //assert
        var lesson = await ReadDbContext.Lessons
            .FirstOrDefaultAsync(cancellationToken);

        result.IsFailure.Should().BeTrue();
        lesson.Should().BeNull();
    }

    private async Task<Guid> SeedModuleToDatabase(
        IssuesWriteDbContext dbContext,
        CancellationToken cancellationToken = default)
    {
        var module = new Module(
            ModuleId.NewModuleId(),
            Title.Create("title").Value,
            Description.Create("description").Value);

        await dbContext.Modules.AddAsync(module, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return module.Id;
    }
}