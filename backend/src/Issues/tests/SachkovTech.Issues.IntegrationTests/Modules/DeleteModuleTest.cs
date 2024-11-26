using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SachkovTech.Issues.Application.Features.Modules.Commands.Delete;

namespace SachkovTech.Issues.IntegrationTests.Modules;

public class DeleteModuleTest : ModulesTestsBase
{
    private readonly ILogger<DeleteModuleHandler> _logger;
    private readonly IValidator<DeleteModuleCommand> _validator;
    
    public DeleteModuleTest(IntegrationTestsWebFactory factory) : base(factory)
    {
        _logger = Scope.ServiceProvider.GetRequiredService<ILogger<DeleteModuleHandler>>();
        _validator = Scope.ServiceProvider.GetRequiredService<IValidator<DeleteModuleCommand>>();
    }

    [Fact]
    public async Task Soft_Delete_Module()
    {
        // act
        var cancellationToken = new CancellationTokenSource().Token;

        var moduleId = await Seeding.AddModuleToDatabase(
            WriteDbContext,
            UnitOfWork,
            cancellationToken);

        var command = new DeleteModuleCommand(moduleId);

        var handler = new DeleteModuleHandler(
            Repository,
            UnitOfWork,
            _validator,
            _logger);
        
        // arrange
        var result = await handler.Handle(command, cancellationToken);
        
        // assert
        result.IsSuccess.Should().BeTrue();
    }
}