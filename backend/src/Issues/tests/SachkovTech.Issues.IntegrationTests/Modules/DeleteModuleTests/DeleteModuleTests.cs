using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SachkovTech.Core.Abstractions;
using SachkovTech.Issues.Application.Features.Modules.Commands.Delete;

namespace SachkovTech.Issues.IntegrationTests.Modules.DeleteModuleTests;

public class DeleteModuleTests: ModuleTestsBase
{
    public DeleteModuleTests(ModuleTestWebFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async Task DeleteModule_should_be_success()
    {
        // Arrange
        var cancellationToken = new CancellationTokenSource().Token;
        
        var moduleId = await SeedModule();
        var command = Fixture.CreateDeleteModuleCommand(moduleId);

        var sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, DeleteModuleCommand>>();

        // Act
        var result = await sut.Handle(command, cancellationToken);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        var module = await ReadDbContext.Modules
            .FirstOrDefaultAsync(x => x.Id == moduleId, cancellationToken);
        
        module.Should().NotBeNull();
        module?.IsDeleted.Should().BeTrue();
    }
}