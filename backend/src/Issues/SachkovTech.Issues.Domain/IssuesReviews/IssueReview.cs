﻿using CSharpFunctionalExtensions;
using SachkovTech.Issues.Domain.IssuesReviews.Entities;
using SachkovTech.Issues.Domain.IssuesReviews.Enums;
using SachkovTech.Issues.Domain.IssuesReviews.Events;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Issues.Domain.IssuesReviews;

public sealed class IssueReview : DomainEntity<IssueReviewId>
{
    // ef core
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private IssueReview(IssueReviewId id) : base(id){}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public IssueReview(
        IssueReviewId issueReviewId,
        UserIssueId userIssueId,
        Guid userId,
        PullRequestUrl pullRequestUrl)
        : base(issueReviewId)
    {
        UserIssueId = userIssueId;
        UserId = userId;
        IssueReviewStatus = IssueReviewStatus.WaitingForReviewer;
        ReviewStartedTime = DateTime.UtcNow;
        PullRequestUrl = pullRequestUrl;
    }

    public UserIssueId UserIssueId { get; private set; }
    public Guid UserId { get; private set; }

    public Guid? ReviewerId { get; private set; } = null;

    public IssueReviewStatus IssueReviewStatus { get; private set; }

    private List<Comment> _comments = [];
    public IReadOnlyList<Comment> Comments => _comments;

    public DateTime ReviewStartedTime { get; private set; }
    public DateTime? IssueTakenTime { get; private set; }

    public DateTime? IssueApprovedTime { get; private set; }

    public PullRequestUrl PullRequestUrl { get; private set; }

    public void StartReview(UserId reviewerId)
    {
        ReviewerId = reviewerId;
        IssueReviewStatus = IssueReviewStatus.OnReview;

        IssueTakenTime ??= DateTime.UtcNow;
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

        AddDomainEvent(new IssueSentForRevisionEvent(UserIssueId));

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

        if (UserId != userId && ReviewerId != null && ReviewerId != userId
            || comment.UserId != userId)
        {
            return Errors.General.ValueIsInvalid("userId");
        }

        _comments.Remove(comment);

        return UnitResult.Success<Error>();
    }
}