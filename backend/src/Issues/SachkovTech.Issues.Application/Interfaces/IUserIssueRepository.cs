using CSharpFunctionalExtensions;
using SachkovTech.Issues.Domain.IssueSolving.Entities;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Issues.Application.Interfaces;

public interface IUserIssueRepository
{
    Task<Guid> Add(UserIssue userIssue, CancellationToken cancellationToken = default);

    Task<Result<UserIssue, Error>> GetUserIssueById(UserIssueId userIssueId,
            CancellationToken cancellationToken = default);
}
