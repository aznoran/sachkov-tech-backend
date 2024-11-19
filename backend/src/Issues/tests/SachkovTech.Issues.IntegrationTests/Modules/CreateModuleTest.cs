using FluentAssertions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using SachkovTech.Issues.Application.Features.Modules.Commands.Create;

namespace SachkovTech.Issues.IntegrationTests.Modules;

public class CreateModuleTest : ModulesTestsBase
{
    private readonly Mock<ILogger<CreateModuleHandler>> _loggerMock = new();
    
    public CreateModuleTest(IntegrationTestsWebAppFactory factory) : base(factory)
    {
        
    }

    [Fact]
    public async Task Create_Module_Should_Add_To_Database()
    {
        // act
        var validator = Scope.ServiceProvider.GetRequiredService<IValidator<CreateModuleCommand>>();
        var cancellationToken = new CancellationTokenSource().Token;

        var command = new CreateModuleCommand("Title", "Description");

        var handler = new CreateModuleHandler(
            Repository,
            UnitOfWork,
            validator,
            _loggerMock.Object);
        
        // arrange
        var result = await handler.Handle(command, cancellationToken);
        
        // assert
        var module = await ReadDbContext.Modules
            .FirstOrDefaultAsync(m => m.Id == result.Value, cancellationToken);

        result.IsSuccess.Should().BeTrue();
        module.Should().NotBeNull();
        module?.Title.Should().Be(command.Title);
    }
}