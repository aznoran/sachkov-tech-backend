using FluentAssertions;
using SachkovTech.Issues.Domain.Module;
using SachkovTech.Issues.Domain.Module.Entities;
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

    [Fact]
    public void Soft_delete_issues_and_this_module()
    {
        // Arrange
        var module = CreateAndFillModule(3);

        // Act
        module.SoftDelete();

        // Assert
        module.IsDeleted.Should().BeTrue();

        foreach (var issue in module.Issues)
        {
            issue.IsDeleted.Should().BeTrue();
            issue.DeletionDate.Should().NotBeNull();
        }
    }

    [Fact]
    public void Restore_issues_and_this_module()
    {
        var module = CreateAndFillModule(3);
        module.SoftDelete();

        // Act
        module.Restore();

        // Assert
        module.IsDeleted.Should().BeFalse();

        foreach (var issue in module.Issues)
        {
            issue.IsDeleted.Should().BeFalse();
            issue.DeletionDate.Should().BeNull();
        }
    }

    [Fact]
    public void Get_issue_by_id_from_module()
    {
        // Arrange
        var module = CreateAndFillModule(0);

        var issueId = IssueId.NewIssueId();
        var issue = new Issue(
            issueId,
            Title.Create("Issue Title").Value,
            Description.Create("Issue Description").Value,
            LessonId.NewLessonId(),
            Experience.Create(1).Value);

        module.AddIssue(issue);

        // Act
        var result = module.GetIssueById(issueId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        issueId.Value.Should().Be(result.Value.Id);
    }

    [Fact]
    public void Get_issue_by_id_from_module_on_issue_is_null()
    {
        // Arrange
        var module = CreateAndFillModule(0);
        var nonExistentIssueId = IssueId.NewIssueId();

        // Act
        var result = module.GetIssueById(nonExistentIssueId);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void Update_issue_main_info_from_module()
    {
        // Arrange
        var module = CreateAndFillModule(0);

        var issueId = IssueId.NewIssueId();
        var issue = new Issue(
            issueId,
            Title.Create("Issue Title").Value,
            Description.Create("Issue Description").Value,
            LessonId.NewLessonId(),
            Experience.Create(1).Value);

        module.AddIssue(issue);

        var newTitle = Title.Create("New Title").Value;
        var newDescription = Description.Create("New Description").Value;
        var newLessonId = LessonId.NewLessonId();
        var newExperience = Experience.Create(2).Value;

        // Act
        var result = module.UpdateIssueInfo(issueId, newTitle, newDescription, newLessonId, newExperience);

        // Assert
        result.IsSuccess.Should().BeTrue();
        issue.Title.Value.Should().Be(newTitle.Value);
        issue.Description.Value.Should().Be(newDescription.Value);
        issue.LessonId.Should().Be(newLessonId);
        issue.Experience.Value.Should().Be(newExperience.Value);
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
                LessonId.Empty,
                Experience.Create(1).Value,
                null);

            module.AddIssue(issue);
        }

        return module;
    }
}