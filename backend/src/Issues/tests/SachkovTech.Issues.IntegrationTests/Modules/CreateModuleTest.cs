using AutoFixture;
using FluentAssertions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SachkovTech.Issues.Application.Features.Modules.Commands.Create;

namespace SachkovTech.Issues.IntegrationTests.Modules;

public class CreateModuleTest : ModulesTestsBase
{
    private readonly ILogger<CreateModuleHandler> _logger;
    private readonly IValidator<CreateModuleCommand> _validator;
    
    public CreateModuleTest(IntegrationTestsWebFactory factory) : base(factory)
    {
        _logger = Scope.ServiceProvider.GetRequiredService<ILogger<CreateModuleHandler>>();
        _validator = Scope.ServiceProvider.GetRequiredService<IValidator<CreateModuleCommand>>();
    }

    [Fact]
    public async Task Add_Module_To_Database()
    {
        // act
        var cancellationToken = new CancellationTokenSource().Token;
        
        var fixture = new Fixture();

        var command = fixture.Create<CreateModuleCommand>();

        var handler = new CreateModuleHandler(
            Repository,
            UnitOfWork,
            _validator,
            _logger);
        
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