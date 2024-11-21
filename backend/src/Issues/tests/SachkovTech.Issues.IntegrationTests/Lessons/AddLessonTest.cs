using AutoFixture;
using CSharpFunctionalExtensions;
using FileService.Communication;
using FileService.Contracts;
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
using System.Threading;

namespace SachkovTech.Issues.IntegrationTests.Lessons;

public class AddLessonTest : LessonsTestsBase
{
    private readonly ILogger<AddLessonHandler> _logger;
    private readonly IValidator<AddLessonCommand> _validator;
    private readonly IFileService _fileService;
    
    public AddLessonTest(IntegrationTestsWebAppFactory factory) : base(factory)
    {
        _logger = Scope.ServiceProvider.GetRequiredService<ILogger<AddLessonHandler>>();
        _validator = Scope.ServiceProvider.GetRequiredService<IValidator<AddLessonCommand>>();
        _fileService = SetFileServiceMock();
    }    

    [Fact]
    public async Task Add_Lesson_To_Database()
    {
        // arrange
        var cancellationToken = new CancellationTokenSource().Token;
        var fixture = new Fixture();

        var moduleId = await AddModuleToDatabase(WriteDbContext, cancellationToken);

        var command = fixture.Build<AddLessonCommand>().With(c => c.ModuleId, moduleId).Create();

        var handler = new AddLessonHandler(
            ReadDbContext,
            _validator,
            Repository,
            _fileService,
            UnitOfWork,
            _logger);
        
        // act
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

    private IFileService SetFileServiceMock()
    {
        var fileServiceMock = new Mock<IFileService>();

        var response = new FileResponse(Guid.NewGuid(), "testUrl");

        fileServiceMock
            .Setup(f => f.CompleteMultipartUpload(It.IsAny<CompleteMultipartRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success<FileResponse, string>(response));

        return fileServiceMock.Object;
    }
}