using FluentAssertions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using SachkovTech.Issues.Application.Features.Lessons.Command.AddLesson;
using SachkovTech.Issues.Domain.Module;
using SachkovTech.Issues.Infrastructure.DbContexts;
using SachkovTech.SharedKernel.ValueObjects;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Issues.IntegrationTests.Lessons;

public class AddLessonTest : LessonsTestsBase
{
    private readonly Mock<ILogger<AddLessonHandler>> _loggerMock = new ();
    
    public AddLessonTest(IntegrationTestsWebAppFactory factory) : base(factory)
    {
        
    }
    
    [Fact]
    public async Task Add_Lesson_Should_Add_Lesson_To_Database()
    {
        // act
        var validator = Scope.ServiceProvider.GetRequiredService<IValidator<AddLessonCommand>>();
        var cancellationToken = new CancellationTokenSource().Token;

        var moduleId = await AddModuleToDatabase(WriteDbContext, cancellationToken);

        var guids = new Guid[]
        {
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid()
        };

        var command = new AddLessonCommand(
            moduleId,
            "title",
            "description",
            3,
            Guid.NewGuid(),
            Guid.NewGuid(),
            guids,
            guids);

        var handler = new AddLessonHandler(
            ReadDbContext,
            validator,
            Repository,
            UnitOfWork,
            _loggerMock.Object);
        
        // arrange
        var result = await handler.Handle(command, cancellationToken); 
        
        //assert
        var lesson = await ReadDbContext.Lessons
            .FirstOrDefaultAsync(l => l.Id == result.Value, cancellationToken);
        
        result.IsSuccess.Should().BeTrue();
        lesson.Should().NotBeNull();
        lesson?.Title.Should().Be(command.Title);
    }

    private async Task<Guid> AddModuleToDatabase(
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