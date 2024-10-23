using CSharpFunctionalExtensions;
using SachkovTech.Issues.Domain.IssuesReviews.Entities;
using SachkovTech.Issues.Domain.IssuesReviews.Enums;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Issues.Domain.IssuesReviews;

public sealed class IssueReview : Entity<IssueReviewId>
{
    // ef core
    private IssueReview(IssueReviewId id) : base(id)
    {
    }

    private IssueReview(
        IssueReviewId issueReviewId,
        UserIssueId userIssueId,
        UserId userId,
        IssueReviewStatus issueReviewStatus,
        DateTime reviewStartedTime,
        DateTime? issueApprovedTime,
        PullRequestUrl pullRequestUrl)
        : base(issueReviewId)
    {
        UserIssueId = userIssueId;
        UserId = userId;
        IssueReviewStatus = issueReviewStatus;
        ReviewStartedTime = reviewStartedTime;
        IssueApprovedTime = issueApprovedTime;
        PullRequestUrl = pullRequestUrl;
    }

    public UserIssueId UserIssueId { get; private set; }
    public UserId UserId { get; private set; }

    public UserId? ReviewerId { get; private set; } = null;

    public IssueReviewStatus IssueReviewStatus { get; private set; }

    private List<Comment> _comments = [];
    public IReadOnlyList<Comment> Comments => _comments;

    public DateTime ReviewStartedTime { get; private set; }
    public DateTime? IssueTakenTime { get; private set; }

    public DateTime? IssueApprovedTime { get; private set; }

    public PullRequestUrl PullRequestUrl { get; private set; }

    public static Result<IssueReview, Error> Create(UserIssueId userIssueId,
        UserId userId,
        PullRequestUrl pullRequestUrl)
    {
        return Result.Success<IssueReview, Error>(new(
            IssueReviewId.NewIssueReviewId(),
            userIssueId,
            userId,
            IssueReviewStatus.WaitingForReviewer,
            DateTime.UtcNow,
            null,
            pullRequestUrl));
    }
    public void StartReview(UserId reviewerId)
    {
        ReviewerId = reviewerId;
        IssueReviewStatus = IssueReviewStatus.OnReview;

        if (IssueTakenTime == null)
        {
            IssueTakenTime = DateTime.UtcNow;
        }
    }

    public UnitResult<Error> SendIssueForRevision(UserId reviewerId)
    {
        if (ReviewerId != reviewerId)
        {
            return Errors.User.InvalidCredentials();
        }

        if (IssueReviewStatus != IssueReviewStatus.OnReview)
        {
            return Errors.General.ValueIsInvalid("issue-review-status");
        }

        IssueReviewStatus = IssueReviewStatus.AskedForRevision;

        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> Approve(UserId reviewerId)
    {
        if (ReviewerId != reviewerId)
        {
            return Errors.User.InvalidCredentials();
        }

        if (IssueReviewStatus != IssueReviewStatus.OnReview)
        {
            return Errors.General.ValueIsInvalid("issue-review-status");
        }

        IssueReviewStatus = IssueReviewStatus.Accepted;

        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> AddComment(Comment comment)
    {
        if (comment.UserId != UserId && ReviewerId != null && ReviewerId != comment.UserId)
        {
            return Errors.General.ValueIsInvalid("userId");
        }

        _comments.Add(comment);

        return UnitResult.Success<Error>();
    }
    public UnitResult<Error> DeleteComment(CommentId commentId, UserId userId)
    {
        var comment = _comments.FirstOrDefault(c => c.Id == commentId);

        if (comment is null)
        {
            return Errors.General.NotFound(commentId.Value, "comment_id");
        }

        var a = UserId != userId && ReviewerId != null && ReviewerId != userId;
        var b = comment.UserId != userId;

        if (UserId != userId && ReviewerId != null && ReviewerId != userId
            || comment.UserId != userId)
        {
            return Errors.General.ValueIsInvalid("userId");
        }

        _comments.Remove(comment);

        return UnitResult.Success<Error>();
    }
}