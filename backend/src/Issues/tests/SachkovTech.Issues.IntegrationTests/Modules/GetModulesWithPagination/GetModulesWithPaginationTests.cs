using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SachkovTech.Core.Abstractions;
using SachkovTech.Issues.Application.Features.Modules.Commands.UpdateMainInfo;

namespace SachkovTech.Issues.IntegrationTests.Modules.GetModulesWithPagination;

public class GetModulesWithPaginationTests: ModuleTestsBase
{
    public GetModulesWithPaginationTests(ModuleTestWebFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async Task GetModules_should_return_modules()
    {
        // Arrange
        var cancellationToken = new CancellationTokenSource().Token;

        var moduleId = await SeedModule();
        var command = Fixture.CreateUpdateMainInfoCommand(moduleId);

        var sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, UpdateMainInfoCommand>>();
        // Act
        var result = await sut.Handle(command, cancellationToken);

        //Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();

        var module = await ReadDbContext.Modules
            .FirstOrDefaultAsync(x => x.Id == moduleId, cancellationToken);

        module.Should().NotBeNull();

        module?.Title.Should().Be(command.Title);
        module?.Description.Should().Be(command.Description);
    }
}