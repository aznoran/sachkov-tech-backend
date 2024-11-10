namespace SachkovTech.Issues.UnitTests.Domain;

public class IssueTests
{
    [Fact]
    public void Test1()
    {
    }
    
     // [Fact]
    // public void Get_issue_by_id_from_module()
    // {
    //     // Arrange
    //     var module = CreateAndFillModule(0);
    //
    //     var issueId = IssueId.NewIssueId();
    //     var issue = new Issue(
    //         issueId,
    //         Title.Create("Issue Title").Value,
    //         Description.Create("Issue Description").Value,
    //         LessonId.NewLessonId(),
    //         Experience.Create(1).Value);
    //
    //     module.AddIssue(issue);
    //
    //     // Act
    //     var result = module.GetIssueById(issueId);
    //
    //     // Assert
    //     result.IsSuccess.Should().BeTrue();
    //     issueId.Value.Should().Be(result.Value.Id);
    // }
    
    // [Fact]
    // public void Get_issue_by_id_from_module_on_issue_is_null()
    // {
    //     // Arrange
    //     var module = CreateAndFillModule(0);
    //     var nonExistentIssueId = IssueId.NewIssueId();
    //
    //     // Act
    //     var result = module.GetIssueById(nonExistentIssueId);
    //
    //     // Assert
    //     result.IsFailure.Should().BeTrue();
    // }
    
    // [Fact]
    // public void Update_issue_main_info_from_module()
    // {
    //     // Arrange
    //     var module = CreateAndFillModule(0);
    //
    //     var issueId = IssueId.NewIssueId();
    //     var issue = new Issue(
    //         issueId,
    //         Title.Create("Issue Title").Value,
    //         Description.Create("Issue Description").Value,
    //         LessonId.NewLessonId(),
    //         Experience.Create(1).Value);
    //
    //     module.AddIssue(issue);
    //
    //     var newTitle = Title.Create("New Title").Value;
    //     var newDescription = Description.Create("New Description").Value;
    //     var newLessonId = LessonId.NewLessonId();
    //     var newExperience = Experience.Create(2).Value;
    //
    //     // Act
    //     var result = module.UpdateIssueInfo(issueId, newTitle, newDescription, newLessonId, newExperience);
    //
    //     // Assert
    //     result.IsSuccess.Should().BeTrue();
    //     issue.Title.Value.Should().Be(newTitle.Value);
    //     issue.Description.Value.Should().Be(newDescription.Value);
    //     issue.LessonId.Should().Be(newLessonId);
    //     issue.Experience.Value.Should().Be(newExperience.Value);
    // }
}