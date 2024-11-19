using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using SachkovTech.Issues.Application.Features.Modules.Commands.Delete;

namespace SachkovTech.Issues.IntegrationTests.Modules;

public class DeleteModuleTest : ModulesTestsBase
{
    private readonly Mock<ILogger<DeleteModuleHandler>> _loggerMock = new();
    
    public DeleteModuleTest(IntegrationTestsWebAppFactory factory) : base(factory)
    {
        
    }

    [Fact]
    public async Task Delete_Module_Should_Be_Is_Deleted()
    {
        // act
        var validator = Scope.ServiceProvider.GetRequiredService<IValidator<DeleteModuleCommand>>();
        var cancellationToken = new CancellationTokenSource().Token;

        var moduleId = await Seeding.AddModuleToDatabase(
            WriteDbContext,
            UnitOfWork,
            cancellationToken);

        var command = new DeleteModuleCommand(moduleId);

        var handler = new DeleteModuleHandler(
            Repository,
            UnitOfWork,
            validator,
            _loggerMock.Object);
        
        // arrange
        var result = await handler.Handle(command, cancellationToken);
        
        // assert
        result.IsSuccess.Should().BeTrue();
    }
}