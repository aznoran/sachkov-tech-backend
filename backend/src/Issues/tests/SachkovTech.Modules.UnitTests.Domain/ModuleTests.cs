using FluentAssertions;
using SachkovTech.Issues.Domain.Module;
using SachkovTech.Issues.Domain.Module.ValueObjects;
using SachkovTech.SharedKernel.ValueObjects;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Issues.UnitTests.Domain;

public class ModuleTests
{
    [Fact]
    public void MoveIssue_to_the_same_position()
    {
        // Arrange
        var module = CreateAndFillModule(5);

        var issueToMove = module.IssuesPosition[0];

        // Act
        var result = module.MoveIssue(issueToMove, Position.Create(1).Value);

        // Assert
        result.IsSuccess.Should().BeTrue();
        module.IssuesPosition.Should().HaveCount(5);
    }

    [Fact]
    public void MoveIssue_to_back()
    {
        // Arrange
        var module = new Module(
            ModuleId.NewModuleId(),
            Title.Create("test title").Value,
            Description.Create("test description").Value);

        var issueGuid1 = IssueId.Create(Guid.NewGuid());
        var issueGuid2 = IssueId.Create(Guid.NewGuid());
        var issueGuid3 = IssueId.Create(Guid.NewGuid());

        var position1 = Position.Create(1).Value;
        var position2 = Position.Create(2).Value;
        var position3 = Position.Create(3).Value;

        var issuesPosition = new List<IssuePosition>
        {
            new IssuePosition(issueGuid1, position1),
            new IssuePosition(issueGuid2, position2),
            new IssuePosition(issueGuid3, position3)
        };

        module.UpdateIssuesPosition(issuesPosition);

        var issuesPositions = module.IssuesPosition.OrderBy(i => i.Position.Value).ToList();

        // Act
        var result = module.MoveIssue(issuesPositions[1], Position.Create(1).Value);

        // Assert
        var finalIssues = module.IssuesPosition.OrderBy(i => i.Position.Value).ToList();

        result.IsSuccess.Should().BeTrue();
        finalIssues[0].IssueId.Value.Should().Be(issueGuid2.Value);
        finalIssues[1].IssueId.Value.Should().Be(issueGuid1.Value);
        finalIssues[2].IssueId.Value.Should().Be(issueGuid3.Value);
    }

    [Fact]
    public void MoveIssue_to_forward()
    {
        // Arrange
        var module = new Module(
            ModuleId.NewModuleId(),
            Title.Create("test title").Value,
            Description.Create("test description").Value);

        var issueGuid1 = IssueId.Create(Guid.NewGuid());
        var issueGuid2 = IssueId.Create(Guid.NewGuid());
        var issueGuid3 = IssueId.Create(Guid.NewGuid());

        var position1 = Position.Create(1).Value;
        var position2 = Position.Create(2).Value;
        var position3 = Position.Create(3).Value;

        var issuesPosition = new List<IssuePosition>
        {
            new IssuePosition(issueGuid1, position1),
            new IssuePosition(issueGuid2, position2),
            new IssuePosition(issueGuid3, position3)
        };

        module.UpdateIssuesPosition(issuesPosition);

        var issuesPositions = module.IssuesPosition.OrderBy(i => i.Position.Value).ToList();

        // Act
        var result = module.MoveIssue(issuesPositions[1], Position.Create(3).Value);

        // Assert
        var finalIssues = module.IssuesPosition.OrderBy(i => i.Position.Value).ToList();

        result.IsSuccess.Should().BeTrue();
        finalIssues[0].IssueId.Value.Should().Be(issueGuid1.Value);
        finalIssues[1].IssueId.Value.Should().Be(issueGuid3.Value);
        finalIssues[2].IssueId.Value.Should().Be(issueGuid2.Value);
    }

    [Fact]
    public void MoveIssue_to_last_position()
    {
        // Arrange
        var module = new Module(
            ModuleId.NewModuleId(),
            Title.Create("test title").Value,
            Description.Create("test description").Value);

        var issueGuid1 = IssueId.Create(Guid.NewGuid());
        var issueGuid2 = IssueId.Create(Guid.NewGuid());
        var issueGuid3 = IssueId.Create(Guid.NewGuid());

        var position1 = Position.Create(1).Value;
        var position2 = Position.Create(2).Value;
        var position3 = Position.Create(3).Value;

        var issuesPosition = new List<IssuePosition>
        {
            new IssuePosition(issueGuid1, position1),
            new IssuePosition(issueGuid2, position2),
            new IssuePosition(issueGuid3, position3)
        };

        module.UpdateIssuesPosition(issuesPosition);

        var issuesPositions = module.IssuesPosition.OrderBy(i => i.Position.Value).ToList();

        // Act
        var result = module.MoveIssue(issuesPositions[0], Position.Create(3).Value);

        // Assert
        var finalIssues = module.IssuesPosition.OrderBy(i => i.Position.Value).ToList();

        result.IsSuccess.Should().BeTrue();
        finalIssues[0].IssueId.Value.Should().Be(issueGuid2.Value);
        finalIssues[1].IssueId.Value.Should().Be(issueGuid3.Value);
        finalIssues[2].IssueId.Value.Should().Be(issueGuid1.Value);
    }

    [Fact]
    public void Soft_delete_module()
    {
        // Arrange
        var module = CreateAndFillModule(3);

        // Act
        module.SoftDelete();

        // Assert
        module.IsDeleted.Should().BeTrue();
    }

    [Fact]
    public void Restore_module()
    {
        // Arrange
        var module = CreateAndFillModule(3);
        module.SoftDelete();

        // Act
        module.Restore();

        // Assert
        module.IsDeleted.Should().BeFalse();
    }

    private Module CreateAndFillModule(int issuesCount)
    {
        var module = new Module(
            ModuleId.NewModuleId(),
            Title.Create("test title").Value,
            Description.Create("test description").Value);

        var issuesPosition = new List<IssuePosition>();

        for (int i = 1; i < issuesCount + 1; i++)
        {
            issuesPosition.Add(new IssuePosition(Guid.NewGuid(), Position.Create(i).Value));
        }

        module.UpdateIssuesPosition(issuesPosition);

        return module;
    }
}