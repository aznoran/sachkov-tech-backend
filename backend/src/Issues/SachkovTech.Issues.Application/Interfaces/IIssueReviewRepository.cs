using CSharpFunctionalExtensions;
using SachkovTech.Issues.Domain.IssuesReviews;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Issues.Application.Interfaces;

public interface IIssueReviewRepository
{
    Task<Result<IssueReview, Error>> GetById(IssueReviewId id,
        CancellationToken cancellationToken = default);
    Task<Result<IssueReview, Error>> GetByUserIssueId(UserIssueId id,
        CancellationToken cancellationToken = default);
    Task<UnitResult<Error>> Add(IssueReview issueReview,
        CancellationToken cancellationToken = default);
}
