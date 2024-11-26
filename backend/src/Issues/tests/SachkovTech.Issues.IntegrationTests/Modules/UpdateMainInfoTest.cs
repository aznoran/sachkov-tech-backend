using AutoFixture;
using FluentAssertions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SachkovTech.Issues.Application.Features.Modules.Commands.UpdateMainInfo;

namespace SachkovTech.Issues.IntegrationTests.Modules;

public class UpdateMainInfoTest : ModulesTestsBase
{
    private readonly ILogger<UpdateMainInfoHandler> _logger;
    private readonly IValidator<UpdateMainInfoCommand> _validator;
    
    public UpdateMainInfoTest(IntegrationTestsWebFactory factory) : base(factory)
    {
        _logger = Scope.ServiceProvider.GetRequiredService<ILogger<UpdateMainInfoHandler>>();
        _validator = Scope.ServiceProvider.GetRequiredService<IValidator<UpdateMainInfoCommand>>();
    }

    [Fact]
    public async Task Update_Module_Main_Information()
    {
        // act
        var cancellationToken = new CancellationTokenSource().Token;

        var fixture = new Fixture();

        var moduleId = await Seeding.AddModuleToDatabase(
            WriteDbContext,
            UnitOfWork,
            cancellationToken);

        var command = fixture.Build<UpdateMainInfoCommand>().With(c => c.ModuleId, moduleId).Create();

        var handler = new UpdateMainInfoHandler(
            Repository,
            UnitOfWork,
            _validator,
            _logger);

        // arrange
        var result = await handler.Handle(command, cancellationToken);
        
        // assert
        var module = await ReadDbContext.Modules
            .FirstOrDefaultAsync(m => m.Id == moduleId, cancellationToken);

        result.IsSuccess.Should().BeTrue();
        module?.Title.Should().Be(command.Title);
    }
}