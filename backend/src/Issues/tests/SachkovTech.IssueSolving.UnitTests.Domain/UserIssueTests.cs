using FluentAssertions;
using SachkovTech.Issues.Domain.IssueSolving.Entities;
using SachkovTech.Issues.Domain.IssueSolving.Enums;
using SachkovTech.SharedKernel.ValueObjects;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.IssueSolving.UnitTests.DomainTestProject1;

public class UserIssueTests
{
    [Fact]
    public void Constructor_should_set_initial_values()
    {
        // Arrange
        var userIssueId = UserIssueId.NewIssueId();
        var userId = UserId.NewUserId();
        var issueId = IssueId.NewIssueId();

        // Act
        var userIssue = new UserIssue(userIssueId, userId, issueId);

        // Assert
        userIssue.UserId.Should().Be(userId);
        userIssue.IssueId.Should().Be(issueId);
        userIssue.Status.Should().Be(IssueStatus.AtWork);
        userIssue.StartDateOfExecution.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        userIssue.Attempts.Should().NotBeNull();
        userIssue.PullRequestUrl.Should().Be(PullRequestUrl.Empty);
    }

    [Fact]
    public void SendOnReview_should_update_status_and_set_pull_request_url_when_status_is_at_work()
    {
        // Arrange
        var userIssue = CreateUserIssue();
        var pullRequestUrl = PullRequestUrl.Empty;

        // Act
        var result = userIssue.SendOnReview(pullRequestUrl);

        // Assert
        result.IsSuccess.Should().BeTrue();
        userIssue.Status.Should().Be(IssueStatus.UnderReview);
        userIssue.PullRequestUrl.Should().Be(pullRequestUrl);
    }

    [Fact]
    public void SendOnReview_should_return_error_when_status_is_not_at_work()
    {
        // Arrange
        var userIssue = CreateUserIssue();
        userIssue.SendOnReview(PullRequestUrl.Empty);

        // Act
        var result = userIssue.SendOnReview(PullRequestUrl.Empty);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
        userIssue.Status.Should().Be(IssueStatus.UnderReview);
    }

    [Fact]
    public void SendForRevision_should_update_status_and_increment_attempts_when_status_under_review()
    {
        // Arrange
        var userIssue = CreateUserIssue();
        userIssue.SendOnReview(PullRequestUrl.Empty);

        // Act
        var result = userIssue.SendForRevision();

        // Assert
        result.IsSuccess.Should().BeTrue();
        userIssue.Status.Should().Be(IssueStatus.AtWork);
        userIssue.Attempts.Value.Should().Be(2);
    }

    [Fact]
    public void SendForRevision_should_return_error_when_status_is_not_under_review()
    {
        // Arrange
        var userIssue = CreateUserIssue();

        // Act
        var result = userIssue.SendForRevision();

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
    }

    [Fact]
    public void StopWorking_should_update_status_to_not_at_work_when_status_is_at_work()
    {
        // Arrange
        var userIssue = CreateUserIssue();

        // Act
        var result = userIssue.StopWorking();

        // Assert
        result.IsSuccess.Should().BeTrue();
        userIssue.Status.Should().Be(IssueStatus.NotAtWork);
    }

    [Fact]
    public void StopWorking_should_return_error_when_status_is_not_at_work()
    {
        // Arrange
        var userIssue = CreateUserIssue();
        userIssue.StopWorking();

        // Act
        var result = userIssue.StopWorking();

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
    }

    [Fact]
    public void CompleteIssue_should_update_status_to_completed_and_set_end_date_when_status_is_under_review()
    {
        // Arrange
        var userIssue = CreateUserIssue();
        userIssue.SendOnReview(PullRequestUrl.Empty);

        // Act
        var result = userIssue.CompleteIssue();

        // Assert
        result.IsSuccess.Should().BeTrue();
        userIssue.Status.Should().Be(IssueStatus.Completed);
        userIssue.EndDateOfExecution.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void CompleteIssue_should_return_error_when_status_is_not_under_review()
    {
        // Arrange
        var userIssue = CreateUserIssue();

        // Act
        var result = userIssue.CompleteIssue();

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
    }

    private UserIssue CreateUserIssue()
    {
        return new UserIssue(
            UserIssueId.NewIssueId(),
            UserId.NewUserId(),
            IssueId.NewIssueId());
    }
}