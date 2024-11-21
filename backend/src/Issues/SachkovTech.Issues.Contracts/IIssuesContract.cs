using CSharpFunctionalExtensions;
using SachkovTech.Issues.Contracts.Responses;
using SachkovTech.Issues.Domain.Module.ValueObjects;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Issues.Contracts;

public interface IIssuesContract
{
    Task<Result<IssueResponse, ErrorList>> GetIssueById(
        Guid issueId, CancellationToken cancellationToken = default);

    Task<Result<Guid, ErrorList>> GetIssueByPosition(
        ModuleId moduleId, Position position, CancellationToken cancellationToken = default);
}