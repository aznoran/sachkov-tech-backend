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

    // [Fact]
    // public void MoveIssue_to_back()
    // {
    //     // Arrange
    //     var module = CreateAndFillModule(3);
    //
    //     var issuesPositions = module.IssuesPosition.OrderBy(i => i.Position).ToList();
    //
    //     // Act
    //     var result = module.MoveIssue(issuesPositions[1], Position.Create(1).Value);
    //
    //     // Assert
    //     var finalIssues = module.IssuesPosition.OrderBy(i => i.Position).ToList();
    //
    //     result.IsSuccess.Should().BeTrue();
    //     finalIssues.Should().Equal(new List<IssuePosition> { issuesPositions[1], issuesPositions[0], issuesPositions[2] });
    // }

    // [Fact]
    // public void MoveIssue_to_forward()
    // {
    //     // Arrange
    //     // Arrange
    //     var module = CreateAndFillModule(3);
    //
    //     var issues = module.Issues.OrderBy(i => i.Position).ToList();
    //
    //     // Act
    //     var result = module.MoveIssue(issues[1], Position.Create(3).Value);
    //
    //     // Assert
    //     var finalIssues = module.Issues.OrderBy(i => i.Position).ToList();
    //
    //     result.IsSuccess.Should().BeTrue();
    //     finalIssues.Should().Equal(new List<Issue> { issues[0], issues[2], issues[1] });
    // }

    // [Fact]
    // public void MoveIssue_to_last_position()
    // {
    //     // Arrange
    //     var module = CreateAndFillModule(3);
    //     var issueToMove = module.Issues[0];
    //     var issues = module.Issues.OrderBy(i => i.Position).ToList();
    //
    //     // Act
    //     var result = module.MoveIssue(issueToMove, Position.Create(5).Value);
    //
    //     // Assert
    //     var finalIssues = module.Issues.OrderBy(i => i.Position).ToList();
    //
    //     result.IsSuccess.Should().BeTrue();
    //     finalIssues.Should().Equal(new List<Issue> { issues[1], issues[2], issues[0] });
    // }

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