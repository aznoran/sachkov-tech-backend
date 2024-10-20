using CSharpFunctionalExtensions;
using SachkovTech.IssuesReviews.Contracts.Requests;
using SachkovTech.SharedKernel;

namespace SachkovTech.IssuesReviews.Contracts;

public interface IIssuesReviewsContract
{
    Task<Result<Guid,ErrorList>> AddComment(
        Guid issueReviewId,
        Guid userId,
        AddCommentRequest request,
        CancellationToken cancellationToken = default);
    
    Task<Result<Guid,ErrorList>> CreateIssueReview(
        Guid userIssueId,
        Guid userId,
        CreateIssueReviewRequest request,
        CancellationToken cancellationToken = default);
    
    Task<Result<Guid,ErrorList>> SendIssueReviewForRevision(
        Guid userIssueId,
        CancellationToken cancellationToken = default);
}