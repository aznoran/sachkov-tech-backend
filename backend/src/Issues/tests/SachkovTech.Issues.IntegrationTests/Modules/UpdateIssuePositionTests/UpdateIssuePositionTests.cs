using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SachkovTech.Core.Abstractions;
using SachkovTech.Issues.Application.Features.Modules.Commands.UpdateIssuePosition;
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
    public async Task UpdateIssuePosition_should_move_forth_to_second_position()
    {
        // Arrange
        var cancellationToken = new CancellationTokenSource().Token;

        var moduleId = await SeedModule();
        //seed 5 IssuePositions
        var issueId = await SeedIssuePositions(moduleId, cancellationToken); // return 4th issuePosition
        
        var command = Fixture.CreateUpdateIssuePositionCommand(moduleId, issueId, 2);

        var sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, UpdateIssuePositionCommand>>();
        // Act
        var result = await sut.Handle(command, cancellationToken);

        //Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();

        var module = await ReadDbContext.Modules
            .FirstOrDefaultAsync(x => x.Id == moduleId, cancellationToken);

        module?.IssuesPosition.Should().NotBeNull();
        module?.IssuesPosition.Where(x => x.IssueId == issueId).Select(x => x.Position)
            .Should().Equal(2);
    }

    private async Task<Guid> SeedIssuePositions(Guid moduleId, CancellationToken cancellationToken = default)
    {
        var module = await WriteDbContext.Modules
            .FirstOrDefaultAsync(x => x.Id == moduleId, cancellationToken);
        if (module is  null)
            throw new Exception($"Seeded Module {moduleId} not found, something wrong with DB");
            
        for (var i = 0; i < 4; i++)
        {
            module.AddIssue(IssueId.NewIssueId()); 
        }
        await WriteDbContext.SaveChangesAsync(cancellationToken);
        return module.IssuesPosition[3].IssueId;
    }    
}