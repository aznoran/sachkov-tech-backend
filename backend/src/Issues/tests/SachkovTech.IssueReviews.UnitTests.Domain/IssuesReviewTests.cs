using FluentAssertions;
using SachkovTech.Issues.Domain.IssuesReviews;
using SachkovTech.Issues.Domain.IssuesReviews.Entities;
using SachkovTech.Issues.Domain.IssuesReviews.Enums;
using SachkovTech.Issues.Domain.IssuesReviews.Events;
using SachkovTech.Issues.Domain.IssuesReviews.ValueObjects;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.IssueReviews.UnitTests.Domain;

public class IssuesReviewTests
{
    [Fact]
    public void Start_review_by_reviewer()
    {
        //Arrange
        var validReviewerId = UserId.NewUserId();
        var issueReview = CreateAndFillIssueReview();

        //Act
        issueReview.StartReview(validReviewerId);

        //Assert
        issueReview.ReviewerId.Should().Be(validReviewerId);
        issueReview.IssueReviewStatus.Should().Be(IssueReviewStatus.OnReview);
        issueReview.ReviewerId.Should().NotBeNull();
    }

    [Fact]
    public void Send_issue_for_revision_by_reviewer()
    {
        //Arrange
        var reviewerId = UserId.NewUserId();
        var issueReview = CreateAndFillIssueReview();
        issueReview.StartReview(reviewerId);

        //Act
        var result = issueReview.SendIssueForRevision(reviewerId);

        //Assert
        var domainEvent = issueReview.DomainEvents.SingleOrDefault() as IssueSentForRevisionEvent;

        result.IsSuccess.Should().BeTrue();
        domainEvent.Should().NotBeNull();
        domainEvent!.UserIssueId.Should().Be(issueReview.UserIssueId);
        issueReview.IssueReviewStatus.Should().Be(IssueReviewStatus.AskedForRevision);
    }

    [Fact]
    public void Send_issue_for_revision_invalid_reviewer_id()
    {
        // Arrange
        var validReviewerId = UserId.NewUserId();
        var invalidReviewerId = UserId.NewUserId();
        var issueReview = CreateAndFillIssueReview();
        issueReview.StartReview(validReviewerId);

        // Act
        var result = issueReview.SendIssueForRevision(invalidReviewerId);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(Errors.User.InvalidCredentials());
    }

    [Fact]
    public void Approve_with_valid_reviewer_id()
    {
        // Arrange
        var reviewerId = UserId.NewUserId();

        var issueReview = CreateAndFillIssueReview();
        issueReview.StartReview(reviewerId);

        // Act
        var result = issueReview.Approve(reviewerId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsSuccess.Should().BeTrue();
        issueReview.IssueReviewStatus.Should().Be(IssueReviewStatus.Accepted);
    }

    [Fact]
    public void Approve_with_invalid_reviewer_id()
    {
        // Arrange
        var validReviewerId = UserId.NewUserId();
        var invalidReviewerId = UserId.NewUserId();

        var issueReview = CreateAndFillIssueReview();
        issueReview.StartReview(validReviewerId);

        // Act
        var result = issueReview.Approve(invalidReviewerId);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(Errors.User.InvalidCredentials());
    }

    [Fact]
    public void Add_comment_from_author_or_reviewer()
    {
        // Arrange
        var authorId = UserId.NewUserId();
        var reviewerId = UserId.NewUserId();

        var issueReview = new IssueReview(
            IssueReviewId.NewIssueReviewId(),
            UserIssueId.NewIssueId(),
            authorId,
            PullRequestUrl.Empty);

        issueReview.StartReview(reviewerId);

        var commentFromAuthor = Comment.Create(authorId, Message.Create("Test1").Value);
        var commentFromReviewer = Comment.Create(reviewerId, Message.Create("Test2").Value);

        // Act
        var resultFromReviewer = issueReview.AddComment(commentFromReviewer.Value);
        var resultFromAuthor = issueReview.AddComment(commentFromAuthor.Value);

        // Assert
        resultFromReviewer.IsSuccess.Should().BeTrue();
        resultFromAuthor.IsSuccess.Should().BeTrue();

        issueReview.Comments.Should().Contain(commentFromAuthor.Value);
        issueReview.Comments.Should().Contain(commentFromReviewer.Value);
    }

    [Fact]
    public void Add_comment_invalid_user()
    {
        // Arrange
        var reviewerId = UserId.NewUserId();
        var invalidUserId = UserId.NewUserId();
        var issueReview = CreateAndFillIssueReview();
        issueReview.StartReview(reviewerId);

        var invalidComment = Comment.Create(invalidUserId, Message.Create("Test").Value);

        // Act
        var result = issueReview.AddComment(invalidComment.Value);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(Errors.General.ValueIsInvalid("userId"));
        issueReview.Comments.Should().NotContain(invalidComment.Value);
    }

    [Fact]
    public void Delete_comment_by_author()
    {
        // Arrange
        var userId = UserId.NewUserId();
        var comment = Comment.Create(userId, Message.Create("Test1").Value);
        var issueReview = CreateAndFillIssueReview();
        issueReview.AddComment(comment.Value);

        // Act
        var result = issueReview.DeleteComment(comment.Value.Id, userId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        issueReview.Comments.Should().NotContain(comment.Value);
    }

    [Fact]
    public void Delete_a_non_author_or_reviewer_comment()
    {
        // Arrange
        var authorId = UserId.NewUserId();
        var reviewerId = UserId.NewUserId();
        var otherUserId = UserId.NewUserId();
        var comment = Comment.Create(authorId, Message.Create("Test1").Value);

        var issueReview = CreateAndFillIssueReview();
        issueReview.AddComment(comment.Value);
        issueReview.StartReview(reviewerId);

        // Act
        var result = issueReview.DeleteComment(comment.Value.Id, otherUserId);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(Errors.General.ValueIsInvalid("userId"));
    }

    private IssueReview CreateAndFillIssueReview()
    {
        return new IssueReview(
            IssueReviewId.NewIssueReviewId(),
            UserIssueId.NewIssueId(),
            UserId.NewUserId(),
            PullRequestUrl.Empty);
    }
}