using AutoFixture;
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
    private readonly ILogger<AddLessonHandler> _logger;
    private readonly IValidator<AddLessonCommand> _validator;
    
    public AddLessonTest(IntegrationTestsWebAppFactory factory) : base(factory)
    {
        _logger = Scope.ServiceProvider.GetRequiredService<ILogger<AddLessonHandler>>();
        _validator = Scope.ServiceProvider.GetRequiredService<IValidator<AddLessonCommand>>();
    }
    
    [Fact]
    public async Task Add_Lesson_To_Database()
    {
        // act
        var cancellationToken = new CancellationTokenSource().Token;
        var fixture = new Fixture();

        var moduleId = await AddModuleToDatabase(WriteDbContext, cancellationToken);

        var command = fixture.Build<AddLessonCommand>().With(c => c.ModuleId, moduleId).Create();

        var handler = new AddLessonHandler(
            ReadDbContext,
            _validator,
            Repository,
            UnitOfWork,
            _logger);
        
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