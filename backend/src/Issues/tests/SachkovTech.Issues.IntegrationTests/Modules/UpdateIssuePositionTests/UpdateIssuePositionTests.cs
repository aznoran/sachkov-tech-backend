using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SachkovTech.Core.Abstractions;
using SachkovTech.Issues.Application.Features.Modules.Commands.UpdateMainInfo;
using SachkovTech.Issues.Domain.Module.ValueObjects;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Issues.IntegrationTests.Modules.UpdateIssuePositionTests;

public class UpdateIssuePositionTests: ModuleTestsBase
{
    public UpdateIssuePositionTests(ModuleTestWebFactory factory) : base(factory)
    {
    }
    [Fact]
    public async Task UpdateIssuePosition_should_succeed()
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

    private IssuePosition CreateIssuePosition(Guid issueId)
    {
        return new IssuePosition(IssueId.Create(issueId), Position.Create(1).Value);
    }
}