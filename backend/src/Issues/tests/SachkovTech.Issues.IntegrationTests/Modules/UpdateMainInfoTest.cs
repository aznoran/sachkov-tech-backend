using FluentAssertions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using SachkovTech.Issues.Application.Features.Modules.Commands.UpdateMainInfo;

namespace SachkovTech.Issues.IntegrationTests.Modules;

public class UpdateMainInfoTest : ModulesTestsBase
{
    private Mock<ILogger<UpdateMainInfoHandler>> _loggerMock = new();
    
    public UpdateMainInfoTest(IntegrationTestsWebAppFactory factory) : base(factory)
    {
        
    }

    [Fact]
    public async Task Update_Main_Info_Should_Update()
    {
        // act
        var validator = Scope.ServiceProvider.GetRequiredService<IValidator<UpdateMainInfoCommand>>();
        var cancellationToken = new CancellationTokenSource().Token;

        var moduleId = await Seeding.AddModuleToDatabase(
            WriteDbContext,
            UnitOfWork,
            cancellationToken);
        
        var command = new UpdateMainInfoCommand(moduleId, "Updated title", "Updated description");

        var handler = new UpdateMainInfoHandler(
            Repository,
            UnitOfWork,
            validator,
            _loggerMock.Object);

        // arrange
        var result = await handler.Handle(command, cancellationToken);
        
        // assert
        var module = await ReadDbContext.Modules
            .FirstOrDefaultAsync(m => m.Id == moduleId, cancellationToken);

        result.IsSuccess.Should().BeTrue();
        module?.Title.Should().Be(command.Title);
    }
}