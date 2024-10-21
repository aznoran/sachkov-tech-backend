using CSharpFunctionalExtensions;
using SachkovTech.SharedKernel;

namespace SachkovTech.IssueSolving.Contracts;

public interface IIssueSolvingContract
{
    Task<Result<Guid,ErrorList>> SendIssueForRevision(Guid userIssueId, CancellationToken cancellationToken = default);
    Task<Result<Guid,ErrorList>> Approve(Guid userIssueId, CancellationToken cancellationToken = default);
}