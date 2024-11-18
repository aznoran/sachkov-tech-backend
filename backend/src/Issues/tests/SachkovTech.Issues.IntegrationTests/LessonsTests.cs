using FluentAssertions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using SachkovTech.Core.Abstractions;
using SachkovTech.Issues.Application.Features.Lessons.Command.AddLesson;
using SachkovTech.Issues.Application.Interfaces;
using SachkovTech.Issues.Domain.Module;
using SachkovTech.Issues.Infrastructure.DbContexts;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Issues.IntegrationTests;

public class LessonsTests : IClassFixture<IntegrationTestsWebAppFactory>
{
    private readonly IntegrationTestsWebAppFactory _factory;
    private readonly Mock<ILogger<AddLessonHandler>> _loggerMock = new ();
    

    public LessonsTests(IntegrationTestsWebAppFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Add_Lesson_Should_Add_Lesson_To_Database()
    {
        // act
        using var scope = _factory.Services.CreateScope();
        
        var repository = scope.ServiceProvider.GetRequiredService<ILessonsRepository>();
        var unitOfWork = scope.ServiceProvider.GetRequiredKeyedService<IUnitOfWork>(Modules.Issues);
        var validator = scope.ServiceProvider.GetRequiredService<IValidator<AddLessonCommand>>();
        var writeDbContext = scope.ServiceProvider.GetRequiredService<IssuesWriteDbContext>();
        var readDbContext = scope.ServiceProvider.GetRequiredService<IReadDbContext>();
        var cancellationToken = new CancellationTokenSource().Token;

        var moduleId = await SeedDatabase(writeDbContext, cancellationToken);

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
            readDbContext,
            validator,
            repository,
            unitOfWork,
            _loggerMock.Object);
        
        // arrange
        var result = await handler.Handle(command, cancellationToken); 
        var lesson = await readDbContext.Lessons
            .FirstOrDefaultAsync(l => l.Id == result.Value, cancellationToken);
        
        //assert
        result.IsSuccess.Should().BeTrue();
        lesson.Should().NotBeNull();
        lesson?.Title.Should().Be(command.Title);
    }

    private async Task<Guid> SeedDatabase(
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