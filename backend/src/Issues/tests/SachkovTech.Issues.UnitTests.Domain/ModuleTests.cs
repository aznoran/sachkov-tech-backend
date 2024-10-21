using FluentAssertions;
using SachkovTech.Issues.Domain;
using SachkovTech.Issues.Domain.Entities;
using SachkovTech.Issues.Domain.ValueObjects;
using SachkovTech.SharedKernel.ValueObjects;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Issues.UnitTests.Domain;

public class ModuleTests
{
    [Fact]
    public void MoveIssue_to_the_same_position()
    {
        // Arrange
        var module = CreateAndFillModule(1);

        var issueToMove = module.Issues[0];

        // Act
        var result = module.MoveIssue(issueToMove, Position.Create(1).Value);

        // Assert
        result.IsSuccess.Should().BeTrue();
        module.Issues.Should().HaveCount(1);
    }

    [Fact]
    public void MoveIssue_to_back()
    {
        // Arrange
        var module = CreateAndFillModule(3);

        var issues = module.Issues.OrderBy(i => i.Position).ToList();

        // Act
        var result = module.MoveIssue(issues[1], Position.Create(1).Value);

        // Assert
        var finalIssues = module.Issues.OrderBy(i => i.Position).ToList();

        result.IsSuccess.Should().BeTrue();
        finalIssues.Should().Equal(new List<Issue> { issues[1], issues[0], issues[2] });
    }

    [Fact]
    public void MoveIssue_to_forward()
    {
        // Arrange
        // Arrange
        var module = CreateAndFillModule(3);

        var issues = module.Issues.OrderBy(i => i.Position).ToList();

        // Act
        var result = module.MoveIssue(issues[1], Position.Create(3).Value);

        // Assert
        var finalIssues = module.Issues.OrderBy(i => i.Position).ToList();

        result.IsSuccess.Should().BeTrue();
        finalIssues.Should().Equal(new List<Issue> { issues[0], issues[2], issues[1] });
    }

    [Fact]
    public void MoveIssue_to_last_position()
    {
        // Arrange
        var module = CreateAndFillModule(3);
        var issueToMove = module.Issues[0];
        var issues = module.Issues.OrderBy(i => i.Position).ToList();

        // Act
        var result = module.MoveIssue(issueToMove, Position.Create(5).Value);

        // Assert
        var finalIssues = module.Issues.OrderBy(i => i.Position).ToList();

        result.IsSuccess.Should().BeTrue();
        finalIssues.Should().Equal(new List<Issue> { issues[1], issues[2], issues[0] });
    }

    private Module CreateAndFillModule(int issuesCount)
    {
        var module = new Module(
            ModuleId.NewModuleId(),
            Title.Create("test title").Value,
            Description.Create("test description").Value);

        for (var i = 0; i < issuesCount; i++)
        {
            var issue = new Issue(
                IssueId.NewIssueId(),
                Title.Create("test title").Value,
                Description.Create("test description").Value,
                LessonId.Empty(),
                Experience.Create(1).Value,
                null);

            module.AddIssue(issue);
        }

        return module;
    }
}