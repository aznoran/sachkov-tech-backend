using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SachkovTech.Core.Abstractions;
using SachkovTech.Issues.Application.Features.Modules.Commands.Create;

namespace SachkovTech.Issues.IntegrationTests.Modules.CreateModuleTests;

public class CreateModuleTests : ModuleTestsBase
{
    public CreateModuleTests(ModuleTestWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task CreateModule_should_be_success()
    {
        // Arrange

        var cancellationToken = new CancellationTokenSource().Token;
        var command = Fixture.CreateCreateModuleCommand();

        var sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, CreateModuleCommand>>();

        // Act
        var result = await sut.Handle(command, cancellationToken);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        var modules = await ReadDbContext.Modules
            .ToListAsync(cancellationToken);

        modules.Should().NotBeNull();
        modules.Should().HaveCount(1);
    }
}