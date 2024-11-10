using CSharpFunctionalExtensions;
using SachkovTech.Core.Dtos;
using SachkovTech.Issues.Contracts.Requests;
using SachkovTech.Issues.Contracts.Responses;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Issues.Contracts;

public interface IIssuesContract
{
    Task<Result<IssueResponse, ErrorList>> GetIssueById(
        Guid issueId, CancellationToken cancellationToken = default);

    Task<Result<Guid, ErrorList>> GetIssueByPosition(
        int position, ModuleId moduleId, CancellationToken cancellationToken = default);
}