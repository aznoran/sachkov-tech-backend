using FluentAssertions;
using SachkovTech.IssuesReviews.Domain;
using SachkovTech.IssuesReviews.Domain.Entities;
using SachkovTech.IssuesReviews.Domain.Enums;
using SachkovTech.IssuesReviews.Domain.ValueObjects;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.IssueReviews.UnitTests.Domain;

public class IssuesReviewTests
{
    #region StartReview

    [Fact]
    public void StartReview_should_update_reviewer_and_status_when_called_with_valid_reviewer_id()
    {
        //Arrange
        var validReviewerId = UserId.NewUserId();
        var issueReview = CreateAndFillModule(5);

        //Act
        issueReview.StartReview(validReviewerId);

        //Assert
        issueReview.ReviewerId.Should().Be(validReviewerId);
        issueReview.IssueReviewStatus.Should().Be(IssueReviewStatus.OnReview);
        issueReview.ReviewerId.Should().NotBeNull();
    }

    #endregion

    #region SendIssueForRevision

    [Fact]
    public void
        SendIssueForRevision_should_update_status_to_asked_for_revision_when_called_with_valid_reviewer_and_status()
    {
        //Arrange
        var reviewerid = UserId.NewUserId();
        var issueReview = CreateAndFillModule(5);
        issueReview.StartReview(reviewerid);

        //Act
        var result = issueReview.SendIssueForRevision(reviewerid);

        //Assert
        result.IsSuccess.Should().BeTrue();
        issueReview.IssueReviewStatus.Should().Be(IssueReviewStatus.AskedForRevision);
    }

    [Fact]
    public void SendIssueForRevision_should_return_invalid_credentials_when_reviewer_id_is_invalid()
    {
        // Arrange
        var validReviewerId = UserId.NewUserId();
        var invalidReviewerId = UserId.NewUserId();
        var issueReview = CreateAndFillModule(5);
        issueReview.StartReview(validReviewerId);

        // Act
        var result = issueReview.SendIssueForRevision(invalidReviewerId);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(Errors.User.InvalidCredentials());
    }

    #endregion

    #region Approve

    [Fact]
    public void Approve_should_update_status_to_accepted_when_reviewer_id_is_valid()
    {
        // Arrange
        var reviewerId = UserId.NewUserId();

        var issueReview = CreateAndFillModule(5);
        issueReview.StartReview(reviewerId);

        // Act
        var result = issueReview.Approve(reviewerId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsSuccess.Should().BeTrue();
        issueReview.IssueReviewStatus.Should().Be(IssueReviewStatus.Accepted);
    }

    [Fact]
    public void Approve_should_return_error_when_reviewer_id_is_invalid()
    {
        // Arrange
        var validReviewerId = UserId.NewUserId();
        var invalidReviewerId = UserId.NewUserId();

        var issueReview = CreateAndFillModule(5);
        issueReview.StartReview(validReviewerId);

        // Act
        var result = issueReview.Approve(invalidReviewerId);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(Errors.User.InvalidCredentials());
    }

    #endregion

    #region AddComment

    [Fact]
    public void AddComment_should_add_comment_when_user_is_author_or_reviewer()
    {
        // Arrange
        var authorId = UserId.NewUserId();
        var reviewerId = UserId.NewUserId();
        
        var issueReview = IssueReview.Create(
            UserIssueId.NewIssueId(),
            authorId,
            PullRequestUrl.Empty).Value;
        
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
    public void AddComment_should_return_error_when_user_is_not_author_or_reviewer()
    {
        // Arrange
        var reviewerId = UserId.NewUserId();
        var invalidUserId = UserId.NewUserId();
        var issueReview = CreateAndFillModule(5);
        issueReview.StartReview(reviewerId);

        var invalidComment = Comment.Create(invalidUserId, Message.Create("Test").Value);

        // Act
        var result = issueReview.AddComment(invalidComment.Value);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(Errors.General.ValueIsInvalid("userId"));
        issueReview.Comments.Should().NotContain(invalidComment.Value);
    }

    #endregion

    #region DeleteComment

    [Fact]
    public void DeleteComment_should_allow_user_to_delete_their_own_comment()
    {
        // Arrange
        var userId = UserId.NewUserId();
        var comment = Comment.Create(userId, Message.Create("Test1").Value);
        var issueReview = CreateAndFillModule(5);
        issueReview.AddComment(comment.Value);

        // Act
        var result = issueReview.DeleteComment(comment.Value.Id, userId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        issueReview.Comments.Should().NotContain(comment.Value);
    }

    [Fact]
    public void DeleteComment_should_return_error_when_user_is_not_author_or_reviewer_or_comment_author()
    {
        // Arrange
        var authorId = UserId.NewUserId();
        var reviewerId = UserId.NewUserId();
        var otherUserId = UserId.NewUserId();
        var comment = Comment.Create(authorId, Message.Create("Test1").Value);
        
        var issueReview = CreateAndFillModule(5);
        issueReview.AddComment(comment.Value);
        issueReview.StartReview(reviewerId);

        // Act
        var result = issueReview.DeleteComment(comment.Value.Id, otherUserId);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(Errors.General.ValueIsInvalid("userId"));
    }

    #endregion

    private IssueReview CreateAndFillModule(int issuesCount)
    {
        var issueReview = IssueReview.Create(
            UserIssueId.NewIssueId(),
            UserId.NewUserId(),
            PullRequestUrl.Empty);

        return issueReview.Value;
    }
}